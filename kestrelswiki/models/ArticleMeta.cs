using System.Text.Json.Serialization;

namespace kestrelswiki.models;

public class ArticleMeta
{
    [JsonPropertyName("mirrorOf")] public string? MirrorOf { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("author")] public string? Author { get; set; }
}