using System;
using AspNetCoreSample.Common;
using AspNetCoreSample.Configuration;
using AspNetCoreSample.Filters;
using AspNetCoreSample.Foundation.WeatherApi;
using AspNetCoreSample.Middleware;
using AspNetCoreSample.Options;
using AspNetCoreSample.RouteConstraints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetCoreSample;

public sealed class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostEnvironment;


    public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<WeatherApiOptions>(_configuration.GetSection(WeatherApiOptions.SectionName));
        services.AddSingleton<IWeatherApiConfiguration, WeatherApiConfiguration>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddSingleton<WeatherProfileCacheFilter>();
        services.AddSingleton<ContentLengthFilter>();
        services.AddScoped<IWeatherApiClient, WeatherApiClient>();
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });
        services.AddRouting(options =>
        {
            options.ConstraintMap.Add(UkPostcodeRouteConstraint.RouteConstraintName, typeof(UkPostcodeRouteConstraint));
            options.ConstraintMap.Add(UsZipcodeRouteConstraint.RouteConstraintName, typeof(UsZipcodeRouteConstraint));
        });
        services.AddRoutePatternLogging();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_hostEnvironment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseRouting();
        /*app.Map("/swagger", a => a.Run(async context =>
        {
            await context.Response.WriteAsync("Fucking Swagger.");
        }));*/
        /*app.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("Middleware 1\r\n");
            await next();
            await context.Response.WriteAsync("Middleware 1\r\n");
        });
        app.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("Middleware 2\r\n");
            await next();
            await context.Response.WriteAsync("Middleware 2\r\n");
        });
        app.Map("/swagger", a => a.Run(async context =>
        {
            await context.Response.WriteAsync("Swagger.\r\n");
        }));*/
        app.UseRoutePatternLogging();
        /*app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "{RequestHost} - - [{RequestDate}] {RequestMethod} {RequestPath} {RequestScheme} {StatusCode} {ResponseSizeInBytes}.";
            options.EnrichDiagnosticContext = (context, httpContext) =>
            {
                context.Set("RequestHost", httpContext.Request.Host.Value);
                context.Set("RequestScheme", httpContext.Request.Scheme);
                context.Set("ResponseSizeInBytes", httpContext.Response.ContentLength ?? (object)"-");
                context.Set("RequestDate", DateTime.UtcNow);
            };
        });*/
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}