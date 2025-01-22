namespace kestrelswiki.environment;

public static class Variables
{
    public static readonly string LogPath = Environment.GetEnvironmentVariable("LOG_PATH") ?? ILogger.DefaultPath;

    public static readonly string ContentPath =
        Environment.GetEnvironmentVariable("CONTENT_PATH") ?? Path.Combine("..", "content");

    public static readonly string WebRootPath =
        Environment.GetEnvironmentVariable("WEBROOT_PATH") ?? Path.Combine("..", "wwwroot");

    public static readonly bool EnableFileLogging =
        Environment.GetEnvironmentVariable("FILE_LOGGING")?.ToLowerInvariant() == "true";

    public static readonly string
        WebPageRepo = Environment.GetEnvironmentVariable("WEBPAGE_REPOSITORY") ??
                      "https://github.com/AceOfKestrels/kestrelsnest.git";

    public static readonly string
        ContentRepository = Environment.GetEnvironmentVariable("CONTENT_REPOSITORY") ??
                            "https://github.com/AceOfKestrels/kestrelsnest.git";

    public static readonly string LogDateFormat =
        Environment.GetEnvironmentVariable("LOG_DATE_FORMAT") ?? ILogger.DefaultDateFormat;

    public static class Webpage
    {
        public static readonly string HomeDirectory = Environment.GetEnvironmentVariable("HOME_DIR") ?? "home";

        public static readonly string FrontpageDirectory =
            Environment.GetEnvironmentVariable("FRONTPAGE_DIR") ?? "frontpage";

        public static readonly string ArticleDirectory =
            Environment.GetEnvironmentVariable("ARTICLE_DIR") ?? "article";

        public static readonly string NotFoundDirectory =
            Environment.GetEnvironmentVariable("NOT_FOUND_DIR") ?? "not-found";
    }
}