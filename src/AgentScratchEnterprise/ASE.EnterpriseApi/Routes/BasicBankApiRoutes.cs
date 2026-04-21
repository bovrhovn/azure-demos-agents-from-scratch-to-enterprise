using ASE.Libraries;
using ASE.Libraries.Data;
using ASE.Libraries.General;
using ASE.Libraries.Models;
using ASE.Libraries.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASE.EnterpriseApi.Routes;

public static class EnterpriseApi
{
    internal const string CacheKey = "BankData";

    public static RouteGroupBuilder MapBasicEnterpriseApi(this RouteGroupBuilder group)
    {
        group.MapGet($"/{RouteNames.GetRoute}", LoadDataAsync)
            .Produces<List<BankData>>()
            .WithSummary("Get all bank data")
            .WithDescription("Get all bank data from the internal database.")
            .WithName("LoadDataAsync")
            .WithTags("bank", "get", "data");
        group.MapGet($"/{RouteNames.SearchRoute}", SearchAsync)
            .Produces<List<SearchResult>>()
            .WithSummary("search bank data")
            .WithDescription("Search bank data from the internal database.")
            .WithName("SearchAsync")
            .WithTags("bank", "search", "data");
        return group;
    }

    private static Task<List<BankData>> LoadDataAsync([FromServices] ILogger logger)
    {
        logger.LogInformation("Loading bank data at {DateLoaded}", DateTime.UtcNow);
        var data = BankDataGenerator.GenerateBankDataList(10);
        logger.LogInformation("Loaded {Count} data", data.Count);
        return Task.FromResult(data);
    }

    private static Task<List<SearchResult>> SearchAsync([FromServices] ILogger logger,
        [FromServices] ISearchService data,
        [FromQuery] string query)
    {
        logger.LogInformation("Searching bank data at {DateLoaded} with {Query}",
            DateTime.UtcNow, query);
        var results = data.Search(query);
        logger.LogInformation("Search returned {Count} results for query {Query}", results.Count(), query);
        return Task.FromResult(results.ToList());
    }
}