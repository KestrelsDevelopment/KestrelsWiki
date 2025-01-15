namespace kestrelswiki.logging.logger;

public class FileLogger(LogDomain logDomain, string dateFormat, string logFilePath) : ILogger
{
    public void Write(object message)
    {
    }
}