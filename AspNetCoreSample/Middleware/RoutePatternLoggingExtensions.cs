using AspNetCoreSample.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class RoutePatternLoggingExtensions 
{
    public static IServiceCollection AddRoutePatternLogging(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<RoutePatternLoggingMiddleware>();
        return serviceCollection;
    }

    public static IApplicationBuilder UseRoutePatternLogging(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseMiddleware<RoutePatternLoggingMiddleware>();
        return appBuilder;
    }
}