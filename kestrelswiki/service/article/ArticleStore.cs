using System.Collections.Generic;
using kestrelswiki.models;
using ILogger = kestrelswiki.logging.logger.ILogger;

namespace kestrelswiki.service.article;

public class ArticleStore(ILogger logger) : IArticleStore
{
    private readonly Dictionary<string, Article> _articles = new();

    public Article? Get(string path)
    {
        path = ToStorePath(path);
        if (!path.StartsWith('/')) path = "/" + path;
        if (_articles.TryGetValue(path, out Article? article))
            logger.Debug($"Retrieved article: \"{article.Meta.Title}\" at {path}");
        return article;
    }

    public bool Set(Article article)
    {
        article.Path = ToStorePath(article.Path);
        if (article.Path.EndsWith(".md")) article.Path = article.Path[..^3];
        if (article.Meta.MirrorOf is not null) article.Meta.MirrorOf = ToStorePath(article.Meta.MirrorOf);

        _articles[article.Path] = article;
        logger.Debug($"Saved article: \"{article.Meta.Title}\" at {article.Path}");
        return true;
    }

    public bool Reset()
    {
        _articles.Clear();
        return true;
    }

    protected string ToStorePath(string path)
    {
        path = path.ToLowerInvariant();
        if (path.EndsWith(".md")) path = path[..^3];
        return path;
    }
}