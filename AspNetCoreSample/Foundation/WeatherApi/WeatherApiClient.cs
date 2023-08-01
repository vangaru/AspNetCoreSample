using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreSample.Common;
using AspNetCoreSample.Configuration;
using AspNetCoreSample.Foundation.WeatherApi.DataContracts;
using AspNetCoreSample.Foundation.WeatherApi.UriUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace AspNetCoreSample.Foundation.WeatherApi;

public sealed class WeatherApiClient : IWeatherApiClient
{
    private const string CurrentWeatherEndpoint = @"current.json";
    private const string QueryParameterName = "q";
    private const string ApiKeyParameterName = "key";


    private readonly IWeatherApiConfiguration _configuration;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<WeatherApiClient> _logger;


    public WeatherApiClient(IWeatherApiConfiguration configuration, IJsonSerializer jsonSerializer, ILogger<WeatherApiClient> logger)
    {
        _configuration = configuration;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
    }


    public async Task<OperationResult<WeatherProfileDataContract>> GetWeatherProfileAsync(string query)
    {
        var queryParameters = new Dictionary<string, string> { { QueryParameterName, query } };
        var response = await GetAsync<WeatherProfileDataContract>(CurrentWeatherEndpoint, queryParameters);

        return response;
    }


    private async Task<OperationResult<TResponse>> GetAsync<TResponse>(string endpoint, IDictionary<string, string> queryParameters)
    {
        var result = await SendAsync<TResponse>(endpoint, queryParameters, HttpMethod.Get);

        return result;
    }

    private async Task<OperationResult<TResponse>> SendAsync<TResponse>(string endpoint, IDictionary<string, string> queryParameters, HttpMethod method)
    {
        try
        {
            using var httpClient = new HttpClient();
            var uri = new Uri(_configuration.ApiUrl, endpoint);
            queryParameters[ApiKeyParameterName] = _configuration.ApiKey;
            uri = uri.AddQueryParameters(queryParameters.AsReadOnly());
            _logger.LogDebug("Sending http {httpMethod} request to obtain current weather profile with query {query}.", method.Method, queryParameters[QueryParameterName]);
            _logger.LogTrace("Using {apiKey} api key for the current request.", queryParameters[ApiKeyParameterName]);
            var request = new HttpRequestMessage(method, uri);
            var response = await httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to make {method} request to {uri} because of server responded with {statusCode}, {responseText}.",
                    method, uri, response.StatusCode, responseText);
                var exceptionReason = new WeatherApiExceptionReason(response.StatusCode);

                return OperationResult<TResponse>.CreateUnsuccessful(exceptionReason);
            }

            _logger.LogDebug("Request was sent successfully. Received {statusCode} status code.", response.StatusCode);
            var contractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
            var responseContent = _jsonSerializer.Deserialize<TResponse>(responseText, contractResolver);

            return responseContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to make {method} request to {endpoint}.", method, endpoint);

            return OperationResult<TResponse>.CreateUnsuccessful(new ApplicationExceptionReason());
        }
    }
}