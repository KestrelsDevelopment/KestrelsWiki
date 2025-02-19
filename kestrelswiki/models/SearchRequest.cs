using System.Text.Json.Serialization;

namespace kestrelswiki.models;

public struct SearchRequest
{
    [JsonPropertyName("searchString")]
    public string SearchString { get; set; }
    [JsonPropertyName("currentPage")]
    public string CurrentPage { get; set; }
    [JsonPropertyName("searchHeadings")]
    public bool SearchHeadings { get; set; }
    [JsonPropertyName("searchBody")]
    public bool SearchBody { get; set; }
}