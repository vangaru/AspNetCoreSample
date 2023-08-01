using System;
using System.Collections.Concurrent;
using AspNetCoreSample.Common;
using AspNetCoreSample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSample.Filters;

public sealed class WeatherProfileCacheFilter : IResourceFilter
{
    private static readonly TimeSpan CacheExpirationIntervalInMinutes = TimeSpan.FromMinutes(5);

    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<WeatherProfileCacheFilter> _logger;
    private readonly ConcurrentDictionary<string, WeatherProfile> _cachedWeatherProfilesByPathMap;
    private readonly ConcurrentDictionary<string, DateTime> _lastRequestedDateTimeByBathMap;


    public WeatherProfileCacheFilter(IJsonSerializer jsonSerializer, ILogger<WeatherProfileCacheFilter> logger)
    {
        _jsonSerializer = jsonSerializer;
        _logger = logger;

        _cachedWeatherProfilesByPathMap = new ConcurrentDictionary<string, WeatherProfile>();
        _lastRequestedDateTimeByBathMap = new ConcurrentDictionary<string, DateTime>();
    }


    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (TryGetFromCache(context.HttpContext, out var weatherProfile))
        {
            _logger.LogDebug("Response for the resource at {resourcePath} will be taken from cache.", context.HttpContext.Request.Path.Value);
            context.Result = new ContentResult
            {
                Content = _jsonSerializer.Serialize(weatherProfile),
                ContentType = ContentTypes.Application.Json,
                StatusCode = StatusCodes.Status200OK,
            };
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        if (!context.Canceled && context.Exception == null)
        {
            CacheResponse(context);
        }
    }


    private bool TryGetFromCache(HttpContext httpContext, out WeatherProfile weatherProfile)
    {
        var utcNow = DateTime.UtcNow;
        weatherProfile = null;
        var requestPath = httpContext.Request.Path.Value;
        if (string.IsNullOrEmpty(requestPath))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(requestPath) &&
            _lastRequestedDateTimeByBathMap.TryGetValue(requestPath, out var lastRequestDateTime) &&
            utcNow - lastRequestDateTime <= CacheExpirationIntervalInMinutes &&
            _cachedWeatherProfilesByPathMap.TryGetValue(requestPath, out var cachedProfile))
        {
            weatherProfile = cachedProfile;
            return true;
        }

        return false;
    }

    private void CacheResponse(ResourceExecutedContext context)
    {
        var utcNow = DateTime.UtcNow;
        var requestPath = context.HttpContext.Request.Path.Value;

        if (!string.IsNullOrEmpty(requestPath) && context.Result is OkObjectResult { Value: WeatherProfile weatherProfile })
        {
            _lastRequestedDateTimeByBathMap[requestPath] = utcNow;
            _cachedWeatherProfilesByPathMap[requestPath] = weatherProfile;
            _logger.LogDebug("Response for the resource at {resourcePath} is successfully cached.", requestPath);
        }
    }
}