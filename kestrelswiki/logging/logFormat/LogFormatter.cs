namespace kestrelswiki.logging.logFormat;

public class LogFormatter(string dateFormat) : ILogFormatter
{
    public string Format(LogDomain logDomain, object message)
    {
        return $"{DateTime.Now.ToString(dateFormat)} [{logDomain.Name}] {message}";
    }
}