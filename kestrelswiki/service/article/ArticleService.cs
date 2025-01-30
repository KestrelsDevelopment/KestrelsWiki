using kestrelswiki.service.file;

namespace kestrelswiki.service.article;

public class ArticleService(ILogger logger, IFileReader fileReader) : IArticleService
{
    public bool Exists(string path)
    {
        return path == "true";
    }
}