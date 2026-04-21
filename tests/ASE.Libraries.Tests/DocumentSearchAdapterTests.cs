using ASE.Libraries;

namespace ASE.Libraries.Tests;

public class DocumentSearchAdapterTests
{
    [Fact]
    public void Search_WithReturnKeyword_ReturnsReturnPolicyResult()
    {
        // Arrange
        const string query = "What is your return policy?";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        var result = Assert.Single(results);
        Assert.Equal("Contoso Outdoors Return Policy", result.SourceName);
        Assert.Equal("https://contoso.com/policies/returns", result.SourceLink);
        Assert.Contains("30 days", result.Text);
        Assert.Contains("refunds", result.Text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Search_WithRefundKeyword_ReturnsReturnPolicyResult()
    {
        // Arrange
        const string query = "How do I get a refund?";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        var result = Assert.Single(results);
        Assert.Equal("Contoso Outdoors Return Policy", result.SourceName);
        Assert.Contains("Refunds are issued", result.Text);
    }

    [Fact]
    public void Search_WithCaseInsensitiveReturnKeyword_ReturnsReturnPolicyResult()
    {
        // Arrange
        const string query = "RETURN information";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
    }

    [Fact]
    public void Search_WithCaseInsensitiveRefundKeyword_ReturnsReturnPolicyResult()
    {
        // Arrange
        const string query = "REFUND policy";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
    }

    [Fact]
    public void Search_WithUnrelatedQuery_ReturnsEmpty()
    {
        // Arrange
        const string query = "What is the weather today?";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Search_WithEmptyString_ReturnsEmpty()
    {
        // Arrange
        const string query = "";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Search_WithNullQuery_ThrowsArgumentNullException()
    {
        // Arrange
        string? query = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DocumentSearchAdapter.Search(query!).ToList());
    }

    [Fact]
    public void SearchResult_CanBeInitialized()
    {
        // Arrange & Act
        var searchResult = new SearchResult
        {
            SourceName = "Test Source",
            SourceLink = "https://test.com",
            Text = "Test content"
        };

        // Assert
        Assert.Equal("Test Source", searchResult.SourceName);
        Assert.Equal("https://test.com", searchResult.SourceLink);
        Assert.Equal("Test content", searchResult.Text);
    }

    [Fact]
    public void SearchResult_DefaultValues_AreEmptyStrings()
    {
        // Arrange & Act
        var searchResult = new SearchResult();

        // Assert
        Assert.Equal(string.Empty, searchResult.SourceName);
        Assert.Equal(string.Empty, searchResult.SourceLink);
        Assert.Equal(string.Empty, searchResult.Text);
    }

    [Theory]
    [InlineData("return")]
    [InlineData("Return")]
    [InlineData("RETURN")]
    [InlineData("refund")]
    [InlineData("Refund")]
    [InlineData("REFUND")]
    public void Search_WithVariousCasing_ReturnsResults(string keyword)
    {
        // Arrange
        var query = $"I need information about {keyword}";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
    }

    [Fact]
    public void Search_WithBothReturnAndRefundKeywords_ReturnsResult()
    {
        // Arrange
        const string query = "Can I return an item for a refund?";

        // Act
        var results = DocumentSearchAdapter.Search(query).ToList();

        // Assert
        Assert.NotEmpty(results);
        var result = Assert.Single(results);
        Assert.Equal("Contoso Outdoors Return Policy", result.SourceName);
    }

    [Fact]
    public void SearchResult_TextContent_ContainsExpectedInformation()
    {
        // Arrange & Act
        var results = DocumentSearchAdapter.Search("return policy").ToList();

        // Assert
        Assert.NotEmpty(results);
        var result = results.First();
        Assert.Contains("30 days", result.Text);
        Assert.Contains("unused", result.Text);
        Assert.Contains("original packaging", result.Text);
        Assert.Contains("5 business days", result.Text);
    }
}
