using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSample.Middleware;

public sealed class RoutePatternLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RoutePatternLoggingMiddleware> _logger;


    public RoutePatternLoggingMiddleware(ILogger<RoutePatternLoggingMiddleware> logger)
    {
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var currentEndpoint = context.GetEndpoint();
        if (currentEndpoint is null)
        {
            await next(context);
            return;
        }

        if (currentEndpoint is RouteEndpoint routeEndpoint)
        {
            //_logger.LogTrace("Route Pattern: {routePattern}.", routeEndpoint.RoutePattern.RawText);
        }

        await next(context);
    }
}