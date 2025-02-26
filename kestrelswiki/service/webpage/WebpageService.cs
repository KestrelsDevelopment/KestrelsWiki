using System.Net.Mime;
using kestrelswiki.extensions;
using kestrelswiki.service.file;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace kestrelswiki.service.webpage;

public class WebpageService(ILogger logger, IFileReader fileReader, IContentTypeProvider contentTypeProvider)
    : IWebpageService
{
    public Try<PhysicalFileResult> TryGetFile(string path)
    {
        Try<PhysicalFileResult> tri;

        if (path.IsNullOrWhiteSpace())
        {
            tri = FormatErrorMessage(path, "Path is empty.");
        }
        else if (!fileReader.Exists(path).Result)
        {
            tri = FormatErrorMessage(path, "File not found.");
        }
        else
        {
            contentTypeProvider.TryGetContentType(path, out string? contentType);
            tri = new PhysicalFileResult(path, contentType ?? MediaTypeNames.Text.Plain);
        }

        tri.Catch(e => logger.Error(FormatErrorMessage(path, e.Message)));

        return tri.Result is not null
            ? tri.Result
            : FormatErrorMessage(path, tri.Exception?.Message);
    }

    private Exception FormatErrorMessage(string? path, string? message)
    {
        return new($"Error getting webpage{(path is not null ? $"at {path}" : "")}: {message}");
    }
}