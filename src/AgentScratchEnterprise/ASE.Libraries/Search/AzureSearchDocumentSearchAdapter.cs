using ASE.Libraries.Models;

namespace ASE.Libraries.Search;

public class AzureSearchDocumentSearchAdapter : ISearchService
{
    public List<SearchResult> Search(string query, int records = 10)
    {
        throw new NotImplementedException();
    }

    public Task<List<SearchResult>> AdvancedSearch(string query, int records = 10)
    {
        throw new NotImplementedException();
    }
}