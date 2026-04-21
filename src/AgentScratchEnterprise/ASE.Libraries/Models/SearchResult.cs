namespace ASE.Libraries.Models;

/// <summary>
/// Represents a single result returned by the document search adapter.
/// </summary>
public sealed class SearchResult
{
    public string SourceName { get; init; } = string.Empty;
    public string SourceLink { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}