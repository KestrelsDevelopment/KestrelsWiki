namespace kestrelswiki.logging.logger;

public interface ILogger
{
    public const string DefaultPath = "../logs";
    public const string DefaultDateFormat = "dd/MM/yyyy HH:mm:ss";

    void Write(object message);
}