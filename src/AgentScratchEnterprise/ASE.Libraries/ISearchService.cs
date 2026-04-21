using ASE.Libraries.Models;

namespace ASE.Libraries;

public interface ISearchService
{
    IEnumerable<SearchResult> Search(string query, int records = 10);
}