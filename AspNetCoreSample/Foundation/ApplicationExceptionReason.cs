using AspNetCoreSample.Common;

namespace AspNetCoreSample.Foundation;

public sealed class ApplicationExceptionReason : IExceptionReason
{
    public ExceptionReasonType Type { get; }

    public string Message { get; }


    public ApplicationExceptionReason()
    {
        Type = ExceptionReasonType.ServerError;
        Message = null;
    }
}