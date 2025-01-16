namespace kestrelswiki.environment;

public static class Variables
{
    public static readonly string LogPath = Environment.GetEnvironmentVariable("LOG_PATH") ?? ILogger.DefaultPath;
    public static readonly string ContentPath = Environment.GetEnvironmentVariable("CONTENT_PATH") ?? "../content";
    public static readonly string WebRootPath = Environment.GetEnvironmentVariable("WEBROOT_PATH") ?? "../wwwroot";

    public static readonly string
        WebPageRepo = Environment.GetEnvironmentVariable("WEBPAGE_REPOSITORY") ?? string.Empty;

    public static readonly string LogDateFormat =
        Environment.GetEnvironmentVariable("LOG_DATE_FORMAT") ?? ILogger.DefaultDateFormat;
}