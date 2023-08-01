using System;
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
[ServiceFilter(typeof(WeatherProfileCacheFilter))]
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
    public async Task<ActionResult<WeatherProfile>> GetByCityName(string city)
    {
        _logger.LogInformation("Getting weather for the city {city}.", city);
        if (string.IsNullOrEmpty(city))
        {
            const string message = "City must be specified.";
            _logger.LogWarning(message);

            return BadRequest(message);
        }
        var getWeatherProfileResult = await GetWeatherProfileAsync(city);
        if (!getWeatherProfileResult.IsSuccessful)
        {
            _logger.LogWarning("Failed to get weather for the city {city}.", city);
            return ExceptionReason(getWeatherProfileResult.ExceptionReason);
        }
        _logger.LogInformation("Successfully get weather for the city {city}.", city);

        return Ok(getWeatherProfileResult.Result);
    }

    [HttpGet]
    [Route("{zipcode:uszipcode}")]
    public async Task<ActionResult<WeatherProfile>> GetByUsZipCode(string zipcode)
    {
        _logger.LogInformation("Getting weather by US zipcode {zipcode}.", zipcode);
        if (string.IsNullOrEmpty(zipcode))
        {
            const string message = "US zipcode must be specified.";
            _logger.LogWarning(message);

            return BadRequest(message);
        }
        var getWeatherProfileResult = await GetWeatherProfileAsync(zipcode);
        if (!getWeatherProfileResult.IsSuccessful)
        {
            _logger.LogWarning("Failed to get weather by US zipcode {zipcode}.", zipcode);
            return ExceptionReason(getWeatherProfileResult.ExceptionReason);
        }
        _logger.LogInformation("Successfully get weather by US zipcode {zipcode}.", zipcode);

        return Ok(getWeatherProfileResult.Result);
    }

    [HttpGet]
    [Route("{postcode:ukpostcode}")]
    public async Task<ActionResult<WeatherProfile>> GetByUkPostcode(string postcode)
    {
        _logger.LogInformation("Getting weather by UK postcode {postcode}.", postcode);
        if (string.IsNullOrEmpty(postcode))
        {
            const string message = "UK postcode must be specified.";
            _logger.LogWarning(message);

            return BadRequest(message);
        }
        var getWeatherProfileResult = await GetWeatherProfileAsync(postcode);
        if (!getWeatherProfileResult.IsSuccessful)
        {
            _logger.LogInformation("Failed to get weather by UK postcode {postcode}.", postcode);
            return ExceptionReason(getWeatherProfileResult.ExceptionReason);
        }
        _logger.LogInformation("Successfully get weather by UK postcode {postcode}.", postcode);

        return Ok(getWeatherProfileResult.Result);
    }

    [HttpGet]
    public async Task<ActionResult<WeatherProfile>> Get([FromQuery]string query)
    {
        _logger.LogInformation("Getting weather by query {query}.", query);
        if (string.IsNullOrEmpty(query))
        {
            const string message = "Query must be specified.";
            _logger.LogWarning(message);

            return BadRequest(message);
        }
        var getWeatherProfileResult = await GetWeatherProfileAsync(query);
        if (!getWeatherProfileResult.IsSuccessful)
        {
            _logger.LogInformation("Failed to get weather by query {query}.", query);
            return ExceptionReason(getWeatherProfileResult.ExceptionReason);
        }
        _logger.LogInformation("Successfully get weather by query {query}.", query);

        return Ok(getWeatherProfileResult.Result);
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