using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.logger;
using kestrelswiki.service.file;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.logging.loggerFactory;

public class DefaultLoggerFactory(
    ILogFormatter logFormatter,
    string logFilePath,
    IFileWriter fileWriter) : ILoggerFactory
{
    public ILogger CreateLogger(LogDomain logDomain)
    {
        return new MultiLogger([
            new ConsoleLogger(logDomain, logFormatter),
            new FileLogger(logDomain, logFormatter, logFilePath, fileWriter)
        ]);
    }
}