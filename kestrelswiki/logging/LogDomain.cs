namespace kestrelswiki.logging;

public record LogDomain(string Name)
{
    public static readonly LogDomain Startup = new("Startup");
    public static readonly LogDomain Api = new("API");
    public static readonly LogDomain Logging = new("Logging");
}