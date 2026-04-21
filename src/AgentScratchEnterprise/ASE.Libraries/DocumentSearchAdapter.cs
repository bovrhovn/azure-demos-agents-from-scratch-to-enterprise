namespace ASE.Libraries;

/// <summary>
/// Represents a single result returned by the document search adapter.
/// </summary>
public sealed class SearchResult
{
    public string SourceName { get; init; } = string.Empty;
    public string SourceLink { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}

/// <summary>
/// Provides a mock document-search back-end for the RAG text-search sample.
/// In production this would call an actual search index (e.g. Azure AI Search).
/// </summary>
public static class DocumentSearchAdapter
{
    /// <summary>
    /// Returns matching document snippets for the supplied query.
    /// Currently, supports return/refund policy lookups.
    /// </summary>
    /// <returns>
    ///     A list of <see cref="SearchResult"/> objects matching the query.
    /// </returns>
    public static IEnumerable<SearchResult> Search(string query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.Contains("return", StringComparison.OrdinalIgnoreCase) ||
            query.Contains("refund", StringComparison.OrdinalIgnoreCase))
        {
            yield return new SearchResult
            {
                SourceName = "Contoso Outdoors Return Policy",
                SourceLink = "https://contoso.com/policies/returns",
                Text = "Customers may return any item within 30 days of delivery. " +
                       "Items should be unused and include original packaging. " +
                       "Refunds are issued to the original payment method within 5 business days of inspection."
            };
        }
    }
}
