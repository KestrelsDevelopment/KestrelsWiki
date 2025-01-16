namespace kestrelswiki.logging;

public class Try<T>
{
    public Try(T? result)
    {
        Result = result;
    }

    public Try(Exception? exception)
    {
        Exception = exception;
    }

    public T? Result { get; }
    public Exception? Exception { get; }
    public bool Success => Exception is null && Result is not null;

    public static Try<T> Fail(string errorMessage, Exception? innerException = null)
    {
        return new(new Exception(errorMessage, innerException));
    }

    public Try<T> Then(Action<T> action)
    {
        if (Success && Result is not null) action(Result);
        return this;
    }

    public Try<T> Catch(Action<Exception> action)
    {
        if (!Success)
            action(
                Exception ??
                new NullReferenceException($"{GetType().Name}<{typeof(T).Name}> failed. No exception was thrown.")
            );
        return this;
    }
}