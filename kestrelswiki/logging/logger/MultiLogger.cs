using System.Collections.Generic;

namespace kestrelswiki.logging.logger;

public class MultiLogger(IEnumerable<ILogger> loggers) : ILogger
{
    public void Write(object message)
    {
        foreach (ILogger logger in loggers) logger.Write(message);
    }
}