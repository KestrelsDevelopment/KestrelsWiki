using System.Linq;
using System.Text.Json;
using kestrelswiki.extensions;
using Microsoft.Extensions.Logging;

namespace kestrelswiki.logging.logFormat;

public class LogFormatter(string dateFormat) : ILogFormatter
{
    public string Format(LogDomain logDomain, LogLevel logLevel, object[] obj)
    {
        string message = string.Join(' ', obj.Select(FormatDeep));
        return $"{DateTime.Now.ToString(dateFormat)} [{logDomain.Name}{logLevel.DisplayName()}] {message}";
    }

    protected string FormatDeep(object message)
    {
        return message as string ?? JsonSerializer.Serialize(message);
    }
}