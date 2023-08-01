using System.Net;
using AspNetCoreSample.Common;

namespace AspNetCoreSample.Foundation.WeatherApi;

public sealed class WeatherApiExceptionReason : IExceptionReason
{
    public ExceptionReasonType Type { get; }

    public string Message { get; }


    public WeatherApiExceptionReason(HttpStatusCode statusCode, string message = null)
    {
        Type = statusCode == HttpStatusCode.BadRequest
            ? ExceptionReasonType.ClientError
            : ExceptionReasonType.ServerError;
        Message = message;
    }
}