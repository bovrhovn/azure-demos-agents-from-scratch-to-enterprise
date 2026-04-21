using ASE.Libraries.Models;

namespace ASE.Libraries;

public class AzureSearchDocumentSearchAdapter : ISearchService
{
    public IEnumerable<SearchResult> Search(string query, int records = 10)
    {
        throw new NotImplementedException();
    }
}