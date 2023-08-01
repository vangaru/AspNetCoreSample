namespace AspNetCoreSample.Common;

public interface IExceptionReason
{
    public ExceptionReasonType Type { get; }

    public string Message { get; }
}