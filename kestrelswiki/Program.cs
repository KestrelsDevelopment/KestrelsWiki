using kestrelswiki.environment;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service.file;
using kestrelswiki.service.git;
using kestrelswiki.service.webpage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddScoped<IGitService>(_ => new DevGitService(lf.Create(LogDomain.GitService)));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
        else
        {
            builder.Services.AddScoped<IGitService>(_ => new GitService(lf.Create(LogDomain.GitService)));
        }

        logger.Write("Building host");
        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

    private static ILoggerFactory InitLoggerFactory()
    {
        ILogFormatter logFormatter = new LogFormatter(Variables.LogDateFormat);
        ILogger fallbackLogger = new ConsoleLogger(LogDomain.Logging, logFormatter);
        IFileWriter logWriter = new FileWriter(fallbackLogger);

        return new DefaultLoggerFactory(logFormatter, Variables.LogPath, logWriter);
    }
}