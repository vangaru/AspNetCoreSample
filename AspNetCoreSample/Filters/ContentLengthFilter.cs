using AspNetCoreSample.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreSample.Filters;

public sealed class ContentLengthFilter : IActionFilter
{
    private readonly IJsonSerializer _jsonSerializer;


    public ContentLengthFilter(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }
    
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if ((context.Exception == null || context.ExceptionHandled) && context.Canceled == false && context.Result is OkObjectResult okObjectResult)
        {
            var value = okObjectResult.Value;
            var serializedToBytesValue = _jsonSerializer.SerializeToUtf8Bytes(value);
            var contentLength = serializedToBytesValue.Length;
            context.HttpContext.Response.Headers.ContentLength = contentLength;
        }
    }
}