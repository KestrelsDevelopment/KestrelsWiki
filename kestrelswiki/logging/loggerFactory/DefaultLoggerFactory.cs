using kestrelswiki.logging.logger;
using kestrelswiki.service.file;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.logging.loggerFactory;

public class DefaultLoggerFactory(string logFilePath, string dateFormat, IFileWriter fileWriter) : ILoggerFactory
{
    public ILogger CreateLogger(LogDomain logDomain)
    {
        return new MultiLogger([
            new ConsoleLogger(logDomain, dateFormat),
            new FileLogger(logDomain, logFilePath, dateFormat, fileWriter)
        ]);
    }
}