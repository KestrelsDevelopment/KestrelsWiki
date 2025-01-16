using kestrelswiki.environment;
using kestrelswiki.service.file;

namespace kestrelswiki.service.webpage;

public class WebpageService(ILogger logger, IFileReader fileReader) : IWebpageService
{
    public Try<string> TryGetWebpage(string path)
    {
        Try<string> tri = string.IsNullOrWhiteSpace(path)
            ? new(new ArgumentException(FormatErrorMessage(path, "Path is empty.")))
            : fileReader.TryReadAllText(Path.Combine(Variables.WebRootPath, path));

        tri.Catch(e => logger.Write(FormatErrorMessage(path, e.Message)));
        return tri.Success ? new(tri.Result) : Try<string>.Fail(FormatErrorMessage(path, tri.Exception?.Message));
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