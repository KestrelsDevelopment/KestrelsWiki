using System.Threading.Tasks;
using kestrelswiki.environment;
using kestrelswiki.logging.logFormat;
using kestrelswiki.logging.loggerFactory;
using kestrelswiki.service;
using kestrelswiki.service.article;
using kestrelswiki.service.file;
using kestrelswiki.service.git;
using kestrelswiki.service.webpage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kestrelswiki;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ILoggerFactory lf = InitLoggerFactory();
        ILogger logger = lf.Create(LogDomain.Startup);

        logger.Info("Adding services to container");

        builder.Services.AddSingleton(lf);
        builder.Services.AddScoped<IFileWriter>(_ => new FileWriter(lf.Create(LogDomain.Files)));
        builder.Services.AddScoped<IFileReader>(_ => new FileReader(lf.Create(LogDomain.Files)));
        builder.Services.AddSingleton<IArticleStore>(_ => new ArticleStore(lf.Create(LogDomain.ArticleStore)));
        builder.Services.AddScoped<IArticleService>(s => new ArticleService(
            lf.Create(LogDomain.ArticleService),
            s.GetRequiredService<IFileReader>(),
            s.GetRequiredService<IArticleStore>()
        ));

        builder.Services.AddScoped<IContentTypeProvider, FileExtensionContentTypeProvider>();
        builder.Services.AddScoped<IWebpageService>(s => new WebpageService(
            lf.Create(LogDomain.WebpageService),
            s.GetRequiredService<IFileReader>(),
            s.GetRequiredService<IContentTypeProvider>()
        ));

        builder.Services.AddControllers();

        builder.Services.AddScoped<IGitService>(builder.Environment.IsDevelopment()
            ? s => new DevGitService(lf.Create(LogDomain.GitService), s.GetRequiredService<IFileReader>())
            : s => new GitService(lf.Create(LogDomain.GitService), s.GetRequiredService<IFileWriter>()));

        logger.Info("Building host");
        WebApplication app = builder.Build();

        Services.Init(app.Services);

        app.UseForwardedHeaders(new()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto,
            ForwardLimit = 2
        });

        app.MapControllers();

        using (IServiceScope scope = app.Services.CreateScope())
        {
            var (ok, err) = await scope.ServiceProvider.GetRequiredService<IGitService>()
                .TryPullContentRepositoryAsync();

            if (err != null)
            {
                logger.Critical(err.Message);

                return;
            }

            if (!ok)
            {
                logger.Critical("Could not pull content repository. No error was returned.");

                return;
            }

            scope.ServiceProvider.GetRequiredService<IArticleService>().RebuildIndex();
        }

        await app.RunAsync();
    }

    private static ILoggerFactory InitLoggerFactory()
    {
        ILogFormatter logFormatter = new LogFormatter(Variables.LogDateFormat);
        ILogger fallbackLogger = new ConsoleLogger(LogDomain.Logging, logFormatter);
        IFileWriter logWriter = new FileWriter(fallbackLogger);

        return new DefaultLoggerFactory(logFormatter, Variables.LogPath, logWriter);
    }
}