using kestrelswiki.logging.logFormat;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.logging.loggerFactory;

public interface ILoggerFactory
{
    ILogger CreateLogger(LogDomain logDomain);
}