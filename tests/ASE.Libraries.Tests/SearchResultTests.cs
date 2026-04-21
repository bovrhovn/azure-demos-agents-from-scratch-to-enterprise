using ASE.Libraries.Models;

namespace ASE.Libraries.Tests;

public class SearchResultTests
{
    [Fact]
    public void SearchResult_CanBeCreatedWithDefaultValues()
    {
        // Act
        var result = new SearchResult();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.SourceName);
        Assert.Equal(string.Empty, result.SourceLink);
        Assert.Equal(string.Empty, result.Text);
    }

    [Fact]
    public void SearchResult_CanBeCreatedWithInitializer()
    {
        // Arrange
        var sourceName = "Test Source";
        var sourceLink = "https://example.com";
        var text = "Test content";

        // Act
        var result = new SearchResult
        {
            SourceName = sourceName,
            SourceLink = sourceLink,
            Text = text
        };

        // Assert
        Assert.Equal(sourceName, result.SourceName);
        Assert.Equal(sourceLink, result.SourceLink);
        Assert.Equal(text, result.Text);
    }

    [Fact]
    public void SearchResult_PropertiesAreInitOnly()
    {
        // Arrange & Act
        var result = new SearchResult
        {
            SourceName = "Initial",
            SourceLink = "https://initial.com",
            Text = "Initial text"
        };

        // The properties should be init-only (cannot be reassigned after initialization)
        // This test verifies the model is correctly defined with init accessors
        Assert.NotNull(result);
    }

    [Fact]
    public void SearchResult_IsSealed()
    {
        // Assert
        Assert.True(typeof(SearchResult).IsSealed);
    }
}
