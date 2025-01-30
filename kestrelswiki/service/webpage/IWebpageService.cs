using Microsoft.AspNetCore.Mvc;

namespace kestrelswiki.service.webpage;

public interface IWebpageService
{
    Try<PhysicalFileResult> TryGetFile(string path);
}