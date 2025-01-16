using kestrelswiki.logging.logFormat;

namespace kestrelswiki.logging.loggerFactory;

public interface ILoggerFactory
{
    ILogger Create(LogDomain logDomain);
}