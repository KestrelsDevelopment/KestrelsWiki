namespace kestrelswiki.service.webpage;

public interface IWebpageService
{
    Try<string> TryGetWebpage(string path);
    bool CloneGitRepository(string url);
}