using Microsoft.Extensions.Logging;

namespace kestrelswiki.logging.logger;

public interface ILogger
{
    public const string DefaultPath = "../logs";
    public const string DefaultDateFormat = "dd/MM/yyyy HH:mm:ss";

    void Write(LogLevel logLevel = LogLevel.Information, params object[] message);

    public void Trace(params object[] message)
    {
        Write(LogLevel.Trace, message);
    }

    public void Debug(params object[] message)
    {
        Write(LogLevel.Debug, message);
    }

    public void Info(params object[] message)
    {
        Write(LogLevel.Information, message);
    }

    public void Warning(params object[] message)
    {
        Write(LogLevel.Warning, message);
    }

    public void Error(params object[] message)
    {
        Write(LogLevel.Error, message);
    }

    public void Critical(params object[] message)
    {
        Write(LogLevel.Critical, message);
    }
}