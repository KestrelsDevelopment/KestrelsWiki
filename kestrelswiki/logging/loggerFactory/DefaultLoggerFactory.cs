using kestrelswiki.logging.logger;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.logging.loggerFactory;

public class DefaultLoggerFactory(string logFilePath, string dateFormat) : ILoggerFactory
{
    public ILogger CreateLogger(LogDomain logDomain)
    {
        return new MultiLogger([
            new ConsoleLogger(logDomain, dateFormat),
            new FileLogger(logDomain, logFilePath, dateFormat)
        ]);
    }
}