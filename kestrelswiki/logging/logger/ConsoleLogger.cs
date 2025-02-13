using kestrelswiki.environment;
using kestrelswiki.logging.logFormat;
using Microsoft.Extensions.Logging;

namespace kestrelswiki.logging.logger;

public class ConsoleLogger(LogDomain logDomain, ILogFormatter logFormatter) : ILogger
{
    public void Write(LogLevel logLevel, params object[] message)
    {
        if (logLevel < Variables.LogLevel) return;
        if (Variables.DisabledLogDomains.Contains(logDomain.Name)) return;
        string formattedMessage = logFormatter.Format(logDomain, logLevel, message);
        Console.WriteLine(formattedMessage);
    }
}