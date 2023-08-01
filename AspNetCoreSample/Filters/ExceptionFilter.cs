using AspNetCoreSample.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSample.Filters;

public sealed class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;


    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }


    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return;
        }
        _logger.LogError(context.Exception, "An unhandled exception was thrown by the application.");
        context.Result = new ContentResult
        {
            Content = "Internal server error",
            ContentType = ContentTypes.Text.Plain,
            StatusCode = StatusCodes.Status500InternalServerError,
        };
        context.ExceptionHandled = true;
    }
}