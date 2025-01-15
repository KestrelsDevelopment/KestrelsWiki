using kestrelswiki.logging;
using kestrelswiki.logging.loggerFactory;
using ILogger = kestrelswiki.logging.logger.ILogger;
using ILoggerFactory = kestrelswiki.logging.loggerFactory.ILoggerFactory;

namespace kestrelswiki;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        MultiLoggerFactory loggerFactory = new("../logs/", "dd/MM/yyyy HH:mm:ss");
        ILogger logger = loggerFactory.CreateLogger(LogDomain.Startup);

        logger.Write("Adding services to container");

        builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);
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
}