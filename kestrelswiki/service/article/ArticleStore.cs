using System.Collections.Generic;
using kestrelswiki.models;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.service.article;

public class ArticleStore(ILogger logger) : IArticleStore
{
    private readonly Dictionary<string, Article> _articles = new();

    public Article? Get(string path)
    {
        if (!path.StartsWith('/')) path = "/" + path;
        if (_articles.TryGetValue(path, out Article? article))
            logger.Debug($"Retrieved article: \"{article.Meta.Title}\" at {article.Path}");
        return article;
    }

    public bool Set(Article article)
    {
        _articles[article.Path] = article;
        logger.Debug($"Saved article: \"{article.Meta.Title}\" at {article.Path}");
        return true;
    }

    public bool Reset()
    {
        _articles.Clear();
        return true;
    }
}