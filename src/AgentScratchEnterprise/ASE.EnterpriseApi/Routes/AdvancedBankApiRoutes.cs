using ASE.Libraries.Data;
using ASE.Libraries.General;
using ASE.Libraries.Models;
using ASE.Libraries.Search;
using Microsoft.AspNetCore.Mvc;

namespace ASE.EnterpriseApi.Routes;

public static class AdvancedEnterpriseApi
{
    public static RouteGroupBuilder MapAdvancedEnterpriseApi(this RouteGroupBuilder group)
    {
        group.MapGet($"/{RouteNames.SearchRoute}", SearchWithOrchestrationAsync)
            .Produces<List<SearchResult>>()
            .WithSummary("search bank data with advanced features")
            .WithDescription("Search bank data from the internal database with orchestration.")
            .WithName("SearchWithOrchestrationAsync")
            .WithTags("bank", "search", "data","agents");
        return group;
    }

    private static async Task<List<SearchResult>> SearchWithOrchestrationAsync([FromServices] ILogger logger,
        [FromServices] ISearchService data,
        [FromQuery] string query)
    {
        logger.LogInformation("Searching bank data at {DateLoaded} with {Query}",
            DateTime.UtcNow, query);
        var results = await data.AdvancedSearch(query);
        logger.LogInformation("Search returned {Count} results for query {Query}", results.Count(), query);
        return results;
    }
}