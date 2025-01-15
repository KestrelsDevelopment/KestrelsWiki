namespace kestrelswiki.logging.logger;

public class ConsoleLogger(LogDomain logDomain, string dateFormat) : ILogger
{
    public void Write(object message)
    {
        Console.WriteLine($"{DateTime.Now.ToString(dateFormat)} [{logDomain.Name}] {message}");
    }
}