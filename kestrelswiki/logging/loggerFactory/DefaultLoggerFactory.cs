using kestrelswiki.logging.logFormat;
using kestrelswiki.service.file;

namespace kestrelswiki.logging.loggerFactory;

public class DefaultLoggerFactory(
    ILogFormatter logFormatter,
    string logFilePath,
    IFileWriter fileWriter) : ILoggerFactory
{
    public ILogger Create(LogDomain logDomain)
    {
        return new MultiLogger([
            new ConsoleLogger(logDomain, logFormatter),
            new FileLogger(logDomain, logFormatter, logFilePath, fileWriter)
        ]);
    }
}