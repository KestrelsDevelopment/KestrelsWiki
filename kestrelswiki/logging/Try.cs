using System.Diagnostics.CodeAnalysis;
using kestrelswiki.environment;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service;
using Microsoft.Extensions.DependencyInjection;

namespace kestrelswiki.logging;

public class Try<T> where T : notnull
{
    private Try(T result, Exception? exception = null)
    {
        Result = result;
        Exception = exception;
        LogException(exception);
    }

    private Try(Exception exception)
    {
        Exception = exception;
        LogException(exception);
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

    private static void LogException(Exception? ex)
    {
        if (ex is null) return;

        if (Services.Provider is null) return;

        using IServiceScope scope = Services.Provider.CreateScope();
        ILogger? logger = scope.ServiceProvider.GetService<ILoggerFactory>()?.Create(LogDomain.Try);

        logger?.Warning(
            $"Try<{typeof(T).Name}> was initialized {(ex.Source is null ? "" : $"by {ex.Source}")} with an exception: {ex.Message}");

        if (Variables.LogStacktraces)
            logger?.Warning(ex.StackTrace ?? ex.InnerException?.StackTrace ?? string.Empty);
    }
}