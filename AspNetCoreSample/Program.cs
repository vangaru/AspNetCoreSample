using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AspNetCoreSample;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        using var host = BuildHost(args);
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        AppDomain.CurrentDomain.UnhandledException += (_, e) => LogDomainUnhandledException(e, logger);
        host.Run();
    }


    private static IHost BuildHost(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>();
            })
            .Build();
    }

    private static void LogDomainUnhandledException(UnhandledExceptionEventArgs e, ILogger<Program> logger)
    {
        logger.LogCritical((Exception)e.ExceptionObject, "Some unexpected exception has occured.");
    }
}