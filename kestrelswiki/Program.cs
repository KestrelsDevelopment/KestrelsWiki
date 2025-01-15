using kestrelswiki.logging;
using kestrelswiki.logging.logger;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service.file;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        DefaultLoggerFactory loggerFactory = InitLoggerFactory();
        ILogger logger = loggerFactory.CreateLogger(LogDomain.Startup);

        logger.Write("Adding services to container");

        builder.Services.AddSingleton(loggerFactory);
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

    private static DefaultLoggerFactory InitLoggerFactory()
    {
        string logFilePath = Environment.GetEnvironmentVariable("LOG_PATH") ?? ILogger.DefaultPath;
        string logDateFormat = Environment.GetEnvironmentVariable("LOG_DATE_FORMAT") ?? ILogger.DefaultDateFormat;
        IFileWriter logWriter = new FileWriter(new ConsoleLogger(LogDomain.Logging, logDateFormat));

        return new DefaultLoggerFactory(logFilePath, logDateFormat, logWriter);
    }
}