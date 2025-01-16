using kestrelswiki.logging;
using kestrelswiki.service.file;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.service.webpage;

public class WebpageService(ILogger logger, IFileReader fileReader) : IWebpageService
{
    private readonly string _wwwroot = Environment.GetEnvironmentVariable("WWWROOT") ?? "../wwwroot";

    public Try<string> TryGetWebpage(string path)
    {
        Try<string> tri = string.IsNullOrWhiteSpace(path)
            ? new(new ArgumentException(FormatErrorMessage(path, "Path is empty.")))
            : fileReader.TryReadAllText(Path.Combine(_wwwroot, path));

        tri.Catch(e => logger.Write(FormatErrorMessage(path, e.Message)));
        return tri.Success ? new(tri.Result) : new(new Exception(FormatErrorMessage(path, tri.Exception?.Message)));
    }

    public bool CloneGitRepository(string url)
    {
        throw new NotImplementedException();
    }

    private string FormatErrorMessage(string? path, string? message)
    {
        return $"Error getting webpage{(path is not null ? $"at {path}" : "")}: {message}";
    }
}