using System.Diagnostics.CodeAnalysis;

namespace kestrelswiki.logging;

public class Try<T> where T : notnull
{
    private Try(T result, Exception? exception = null)
    {
        Result = result;
        Exception = exception;
    }

    private Try(Exception exception)
    {
        Exception = exception;
    }

    public static implicit operator Try<T>(T value) => new(value);

    public static implicit operator Try<T>((T value, Exception ex) args) => new(args.value, args.ex);

    public static implicit operator Try<T>((T value, string exMessage, Exception? innerException) args) =>
        new(args.value, new(args.exMessage, args.innerException));

    public static implicit operator Try<T>(Exception ex) => new(ex);

    public T? Result { get; }

    public Exception? Exception { get; }

    [MemberNotNullWhen(true, "Result")]
    [MemberNotNullWhen(false, "Exception")]
    public bool Success => Result is not null;

    public Try<T> Then(Action<T> action)
    {
        if (Result is not null) action(Result);

        return this;
    }

    public Try<T> Catch<TEx>(Action<TEx> action) where TEx : Exception
    {
        if (Exception is TEx tex) action(tex);

        return this;
    }

    public Try<T> Catch(Action<Exception> action)
    {
        return Catch<Exception>(action);
    }

    public void Deconstruct(out T? result, out Exception? exception)
    {
        result = Result;
        exception = Exception;
    }
}