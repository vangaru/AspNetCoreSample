using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreSample.Common;
using AspNetCoreSample.Filters;
using AspNetCoreSample.Foundation.WeatherApi;
using AspNetCoreSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSample.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(ContentLengthFilter))]
public class WeatherController : ControllerBase
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly ILogger<WeatherController> _logger;


    public WeatherController(IWeatherApiClient weatherApiClient, ILogger<WeatherController> logger)
    {
        _weatherApiClient = weatherApiClient;
        _logger = logger;
    }

    [HttpGet]
    [Route("{city:alpha:length(3,40):required}")]
    [ServiceFilter(typeof(WeatherProfileCacheFilter))]
    public async Task<ActionResult<WeatherProfile>> GetByCityName(string city)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["city"] = city }))
        {
            _logger.LogInformation("Getting weather for the city.");
            var getWeatherProfileResult = await GetWeatherProfileAsync(city);
            if (!getWeatherProfileResult.IsSuccessful)
            {
                _logger.LogWarning("Failed to get weather for the city.");
                return ExceptionReason(getWeatherProfileResult.ExceptionReason);
            }
            _logger.LogInformation("Successfully get weather for the city.");

            return Ok(getWeatherProfileResult.Result);
        }
    }

    [HttpGet]
    [Route("{zipcode:uszipcode}")]
    public async Task<ActionResult<WeatherProfile>> GetByUsZipCode(string zipcode)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["zipcode"] = zipcode }))
        {
            _logger.LogInformation("Getting weather by US zipcode.");
            var getWeatherProfileResult = await GetWeatherProfileAsync(zipcode);
            if (!getWeatherProfileResult.IsSuccessful)
            {
                _logger.LogWarning("Failed to get weather by US zipcode.");
                return ExceptionReason(getWeatherProfileResult.ExceptionReason);
            }
            _logger.LogInformation("Successfully get weather by US zipcode.");

            return Ok(getWeatherProfileResult.Result);   
        }
    }

    [HttpGet]
    [Route("{postcode:ukpostcode}")]
    public async Task<ActionResult<WeatherProfile>> GetByUkPostcode(string postcode)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["postcode"] = postcode }))
        {
            _logger.LogInformation("Getting weather by UK postcode.");
            var getWeatherProfileResult = await GetWeatherProfileAsync(postcode);
            if (!getWeatherProfileResult.IsSuccessful)
            {
                _logger.LogWarning("Failed to get weather by UK postcode.");
                return ExceptionReason(getWeatherProfileResult.ExceptionReason);
            }
            _logger.LogInformation("Successfully get weather by UK postcode.");   

            return Ok(getWeatherProfileResult.Result);
        }
    }

    [HttpGet]
    public async Task<ActionResult<WeatherProfile>> Get([FromQuery]string query)
    {
        using (_logger.BeginScope(new Dictionary<string, object> { ["query"] = query }))
        {
            _logger.LogInformation("Getting weather by query.");
            if (string.IsNullOrEmpty(query))
            {
                const string message = "Query must be specified.";
                _logger.LogWarning(message);

                return BadRequest(message);
            }
            var getWeatherProfileResult = await GetWeatherProfileAsync(query);
            if (!getWeatherProfileResult.IsSuccessful)
            {
                _logger.LogWarning("Failed to get weather by query.");
                return ExceptionReason(getWeatherProfileResult.ExceptionReason);
            }
            _logger.LogInformation("Successfully get weather by query.");

            return Ok(getWeatherProfileResult.Result);   
        }
    }

    [HttpGet]
    [Route("/exception")]
    public IActionResult GetException()
    {
        var innerException = new ApplicationException("Inner exception.");
        var exception = new ApplicationException("Main error.", innerException);
        throw exception;
    }


    private async Task<OperationResult<WeatherProfile>> GetWeatherProfileAsync(string query)
    {
        var getWeatherProfileResult = await _weatherApiClient.GetWeatherProfileAsync(query);
        if (!getWeatherProfileResult.IsSuccessful)
        {
            return OperationResult<WeatherProfile>.CreateUnsuccessful(getWeatherProfileResult.ExceptionReason);
        }
        var weatherProfile = WeatherProfile.CreateFrom(getWeatherProfileResult.Result);

        return weatherProfile;
    }

    private ActionResult ExceptionReason(IExceptionReason exceptionReason)
    {
        return exceptionReason.Type == ExceptionReasonType.ClientError ? BadRequest(exceptionReason.Message) : InternalServerError();
    }

    private ActionResult InternalServerError()
    {
        return new StatusCodeResult(500);
    }
}