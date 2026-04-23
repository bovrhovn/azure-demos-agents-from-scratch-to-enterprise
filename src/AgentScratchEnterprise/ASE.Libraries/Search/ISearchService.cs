using ASE.Libraries.Models;

namespace ASE.Libraries.Search;

public interface ISearchService
{
    List<SearchResult> Search(string query, int records = 10);
    Task<List<SearchResult>> AdvancedSearch(string query, int records = 10);
}