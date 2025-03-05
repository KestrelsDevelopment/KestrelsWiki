using System.Collections.Generic;
using kestrelswiki.environment;
using Microsoft.Extensions.Logging;

namespace kestrelswiki.logging.logger;

public class MultiLogger(IEnumerable<ILogger> loggers) : ILogger
{
    public void Write(LogLevel logLevel, params object[] message)
    {
        if(message.Length == 0) return;
        if (logLevel < Variables.LogLevel) return;
        foreach (ILogger logger in loggers) logger.Write(logLevel, message);
    }
}