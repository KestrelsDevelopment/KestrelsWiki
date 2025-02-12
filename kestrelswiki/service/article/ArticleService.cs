using System.Collections.Generic;
using System.Text.Json;
using kestrelswiki.environment;
using kestrelswiki.extensions;
using kestrelswiki.models;
using kestrelswiki.service.file;

namespace kestrelswiki.service.article;

public class ArticleService(ILogger logger, IFileReader fileReader, IArticleStore store) : IArticleService
{
    public bool Exists(string path)
    {
        return store.Get(path) is not null;
    }

    public Try<bool> RebuildIndex()
    {
        List<Exception> exceptions = [];

        Try<IEnumerable<Article>> tri = fileReader
            .GetMarkdownFiles(Variables.ContentPath)
            .Then(result =>
            {
                foreach (Article article in result)
                    AddToIndex(article).Catch(exceptions.Add);
            })
            .Catch<AggregateException>(exception =>
            {
                foreach (Exception ex in exception.InnerExceptions)
                    logger.Write(ex.Message);
            })
            .Catch(exception => { logger.Write(exception.Message); });

        return new(tri.Result is not null, new AggregateException(exceptions));
    }

    protected Try<bool> AddToIndex(Article article)
    {
        Try<string> tryReadAllText = fileReader.TryReadAllText(article.Path);
        if (tryReadAllText.Result is null) return new(true);

        Try<(ArticleMeta? meta, string remainingContent)> tryFileMeta = ExtractFileMeta(tryReadAllText.Result);
        article.Meta = tryFileMeta.Result.meta ?? new();
        string fileContent = tryFileMeta.Success
            ? tryFileMeta.Result.remainingContent
            : tryReadAllText.Result;

        Try<IEnumerable<Heading>> tryHeadings = ExtractHeadings(article.Content);
        if (!tryHeadings.Success) return new(new("Heading could not be found", tryHeadings.Exception));

        article.Content = RenderMarkdown(fileContent);
        store.Set(article);

        return new(true);
    }

    protected Try<(ArticleMeta? meta, string remainingContent)> ExtractFileMeta(string content)
    {
        if (content.IsNullOrWhiteSpace()) return new(new("Article is empty"));

        string fileMetaStr = content.Split('\n')[0];
        try
        {
            ArticleMeta? meta = JsonSerializer.Deserialize<ArticleMeta>(fileMetaStr);
            if (meta is null) throw new JsonException("Deserialized result is null");
            return new((new(), content[fileMetaStr.Length..]));
        }
        catch (JsonException e)
        {
            return new(new("No article meta found", e));
        }
    }

    protected Try<IEnumerable<Heading>> ExtractHeadings(string content)
    {
        if (content.IsNullOrWhiteSpace()) return new(new("Article is empty"));
        if (!content.StartsWith("# ")) return new(new("No title found"));

        List<Heading> result = [];
        string[] lines = content.Split('\n');
        foreach (string line in lines)
            if ((line.StartsWith("# ") && result.Count == 0) || line.StartsWith("## ") || line.StartsWith("### "))
                result.Add(new() { Name = line[1..] });

        return new(result);
    }

    protected string RenderMarkdown(string content)
    {
        return content;
    }
}