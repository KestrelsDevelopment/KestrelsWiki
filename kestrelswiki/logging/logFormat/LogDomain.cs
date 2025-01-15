namespace kestrelswiki.logging.logFormat;

public record LogDomain(string Name)
{
    public static readonly LogDomain Startup = new("Startup");
    public static readonly LogDomain Logging = new("Logging");
    public static readonly LogDomain Testing = new("Testing");
}