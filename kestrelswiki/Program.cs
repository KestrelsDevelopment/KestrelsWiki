using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.logger;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service.file;
using kestrelswiki.service.webpage;
using ILogger = kestrelswiki.logging.logger.ILogger;
using ILoggerFactory = kestrelswiki.logging.loggerFactory.ILoggerFactory;

namespace kestrelswiki;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ILoggerFactory lf = InitLoggerFactory();
        ILogger logger = lf.Create(LogDomain.Startup);

        logger.Write("Adding services to container");

        builder.Services.AddSingleton(lf);
        builder.Services.AddScoped<IFileWriter>(_ => new FileWriter(lf.Create(LogDomain.Files)));
        builder.Services.AddScoped<IFileReader>(_ => new FileReader(lf.Create(LogDomain.Files)));
        builder.Services.AddScoped<IWebpageService>(s => new WebpageService(
            lf.Create(LogDomain.WebpageService),
            s.GetRequiredService<IFileReader>()
        ));
        builder.Services.AddControllers();

#if DEBUG
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
#endif

        logger.Write("Building host");
        WebApplication app = builder.Build();

#if DEBUG
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
#endif

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

    private static ILoggerFactory InitLoggerFactory()
    {
        string logFilePath = Environment.GetEnvironmentVariable("LOG_PATH") ?? ILogger.DefaultPath;
        string logDateFormat = Environment.GetEnvironmentVariable("LOG_DATE_FORMAT") ?? ILogger.DefaultDateFormat;
        ILogFormatter logFormatter = new LogFormatter(logDateFormat);
        ILogger fallbackLogger = new ConsoleLogger(LogDomain.Logging, logFormatter);
        IFileWriter logWriter = new FileWriter(fallbackLogger);

        return new DefaultLoggerFactory(logFormatter, logFilePath, logWriter);
    }
}