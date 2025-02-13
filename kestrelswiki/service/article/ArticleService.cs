using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using kestrelswiki.extensions;
using kestrelswiki.models;
using kestrelswiki.service.file;
using Markdig;

namespace kestrelswiki.service.article;

public class ArticleService(ILogger logger, IFileReader fileReader, IArticleStore store) : IArticleService
{
    public bool Exists(string path)
    {
        return store.Get(path) is not null;
    }

    public Try<bool> RebuildIndex()
    {
        logger.Info("Rebuilding index");
        List<Exception> exceptions = [];

        Try<IEnumerable<Article>> tri = fileReader
            .GetMarkdownFiles()
            .Then(result => result.ForEach(article => AddToIndex(article).Catch(exceptions.Add)))
            .Catch<AggregateException>(exception => exception.InnerExceptions.ForEach(ex => logger.Error(ex.Message)))
            .Catch(exception =>
            {
                if (exception is not AggregateException) logger.Error(exception.Message);
            });

        return new(tri.Result is not null, new AggregateException(exceptions));
    }

    protected Try<bool> AddToIndex(Article article)
    {
        ExtractFileMeta(article.Content)
            .Then(((ArticleMeta meta, string content) r) =>
            {
                article.Meta = r.meta;
                article.Content = r.content;
            })
            .Catch(ex => logger.Trace(ex));

        if (article.Meta.MirrorOf is not null)
        {
            store.Set(article);
            return new(true);
        }

        Try<IEnumerable<Heading>> headings = ExtractHeadings(article.Content)
            .Then(result =>
            {
                Heading[] headings = result as Heading[] ?? result.ToArray();
                article.Headings = headings;
                article.Meta.Title ??= headings[0].Name;
            })
            .Catch(ex => logger.Trace(ex));
        if (!headings.Success) return new(new("Title could not be found", headings.Exception));

        article.Content = RenderMarkdown(article.Content);
        store.Set(article);

        return new(true);
    }

    protected Try<(ArticleMeta meta, string remainingContent)> ExtractFileMeta(string content)
    {
        if (content.IsNullOrWhiteSpace()) return new(new("Article is empty"));

        string fileMetaStr = content.Split(Environment.NewLine)[0];
        try
        {
            ArticleMeta? meta = JsonSerializer.Deserialize<ArticleMeta>(fileMetaStr);
            if (meta is null) throw new JsonException("Deserialized result is null");
            return new((meta, content[fileMetaStr.Length..]));
        }
        catch (JsonException e)
        {
            return new((new(), content), new("No article meta found", e));
        }
    }

    protected Try<IEnumerable<Heading>> ExtractHeadings(string content)
    {
        if (content.IsNullOrWhiteSpace()) return new(new("Article is empty"));
        if (!content.StartsWith("# ")) return new(new("No title found"));

        List<Heading> result = [];
        string[] lines = content.Split(Environment.NewLine);

        lines.Where(line =>
                line.Length > 2 &&
                ((line.StartsWith("# ") && result.Count == 0)
                 || line.StartsWith("## ")
                 || line.StartsWith("### ")))
            .ForEach(line => result.Add(new() { Name = line[2..] }));

        return new(result);
    }

    protected string RenderMarkdown(string content)
    {
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(content, pipeline);
    }
}