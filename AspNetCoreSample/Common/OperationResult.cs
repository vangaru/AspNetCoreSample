using System;

namespace AspNetCoreSample.Common;

public class OperationResult<T>
{
    private readonly T _result;


    public bool IsSuccessful { get; }
    
    public IExceptionReason ExceptionReason { get; }


    public T Result
    {
        get
        {
            if (!IsSuccessful)
            {
                throw new InvalidOperationException();
            }

            return _result;
        }
    }


    private OperationResult(T result)
    {
        IsSuccessful = true;
        _result = result;
    }

    private OperationResult(IExceptionReason exceptionReason)
    {
        IsSuccessful = false;
        _result = default;
        ExceptionReason = exceptionReason;
    }


    public static OperationResult<T> CreateUnsuccessful(IExceptionReason exceptionReason)
    {
        return new OperationResult<T>(exceptionReason);
    }

    public static implicit operator OperationResult<T>(T result)
    {
        return CreateSuccessful(result);
    }


    private static OperationResult<T> CreateSuccessful(T result)
    {
        return new OperationResult<T>(result);
    }
}



public static class OperationResult
{
    public static OperationResult<T> CreateSuccessful<T>(T result)
    {
        return result;
    }
}