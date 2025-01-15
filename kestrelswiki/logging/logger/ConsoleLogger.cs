using kestrelswiki.logging.logFormat;

namespace kestrelswiki.logging.logger;

public class ConsoleLogger(LogDomain logDomain, ILogFormatter logFormatter) : ILogger
{
    public void Write(object message)
    {
        Console.WriteLine(logFormatter.Format(logDomain, message));
    }
}