using System.Collections.Generic;
using kestrelswiki.environment;
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
        List<ILogger> loggers = [new ConsoleLogger(logDomain, logFormatter)];
        if (Variables.EnableFileLogging) loggers.Add(new FileLogger(logDomain, logFormatter, logFilePath, fileWriter));
        return new MultiLogger(loggers);
    }
}