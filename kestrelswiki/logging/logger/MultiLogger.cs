namespace kestrelswiki.logging.logger;

public class MultiLogger(ILogger[] loggers) : ILogger
{
    public void Write(object message)
    {
        foreach (var logger in loggers) logger.Write(message);
    }
}