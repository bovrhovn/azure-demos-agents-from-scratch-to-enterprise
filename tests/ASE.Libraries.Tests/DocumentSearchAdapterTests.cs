using ASE.Libraries;
using ASE.Libraries.Models;
using ASE.Libraries.Search;

namespace ASE.Libraries.Tests;

public class DocumentSearchAdapterTests
{
    private readonly DocumentSearchAdapter _adapter;

    public DocumentSearchAdapterTests()
    {
        _adapter = new DocumentSearchAdapter();
    }

    [Fact]
    public void Search_WithReturnQuery_ReturnsReturnPolicyResult()
    {
        // Arrange
        var query = "return policy";

        // Act
        var results = _adapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.SourceName == "Contoso Outdoors Return Policy");
        Assert.Contains(results, r => r.Text.Contains("30 days"));
    }

    [Fact]
    public void Search_WithRefundQuery_ReturnsRefundPolicyResult()
    {
        // Arrange
        var query = "refund policy";

        // Act
        var results = _adapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.SourceName == "Contoso Outdoors Return Policy");
        Assert.Contains(results, r => r.Text.Contains("Refunds"));
    }

    [Fact]
    public void Search_WithReturnQueryCaseInsensitive_ReturnsResult()
    {
        // Arrange
        var query = "RETURN";

        // Act
        var results = _adapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.SourceName == "Contoso Outdoors Return Policy");
    }

    [Fact]
    public void Search_WithAmountQuery_ReturnsBankTransactions()
    {
        // Arrange
        var query = "amount";

        // Act
        var results = _adapter.Search(query, 5).ToList();

        // Assert
        Assert.NotEmpty(results);
        // Should contain at least one transaction result
        Assert.Contains(results, r => r.Text.Contains("transaction"));
    }

    [Fact]
    public void Search_WithAmountQueryAndCustomRecordCount_RespectsRecordLimit()
    {
        // Arrange
        var query = "amount";
        var recordCount = 3;

        // Act
        var results = _adapter.Search(query, recordCount).ToList();

        // Assert
        Assert.NotEmpty(results);
    }

    [Fact]
    public void Search_WithUnmatchedQuery_ReturnsEmptyList()
    {
        // Arrange
        var query = "some unrelated query";

        // Act
        var results = _adapter.Search(query);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Search_ImplementsISearchService()
    {
        // Assert
        Assert.IsAssignableFrom<ISearchService>(_adapter);
    }

    [Fact]
    public void Search_WithEmptyQuery_ReturnsEmptyList()
    {
        // Arrange
        var query = "";

        // Act
        var results = _adapter.Search(query);

        // Assert
        Assert.Empty(results);
    }
}
