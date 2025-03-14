using System.Collections.Generic;

namespace kestrelswiki.models;

public class Article
{
    protected FileInfo? file;
    public string Path { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ArticleMeta Meta { get; set; } = new();
    public IEnumerable<Heading> Headings { get; set; } = [];
}