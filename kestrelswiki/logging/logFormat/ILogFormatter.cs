namespace kestrelswiki.logging.logFormat;

public interface ILogFormatter
{
    string Format(LogDomain logDomain, object message);
}