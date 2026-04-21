using ASE.Libraries;
using ASE.Libraries.Search;

namespace ASE.Libraries.Tests;

public class ISearchServiceTests
{
    [Fact]
    public void ISearchService_IsAnInterface()
    {
        // Assert
        Assert.True(typeof(ISearchService).IsInterface);
    }

    [Fact]
    public void ISearchService_HasSearchMethod()
    {
        // Act
        var method = typeof(ISearchService).GetMethod("Search");

        // Assert
        Assert.NotNull(method);
        Assert.Equal("Search", method.Name);
    }

    [Fact]
    public void DocumentSearchAdapter_ImplementsISearchService()
    {
        // Act
        var adapter = new DocumentSearchAdapter();

        // Assert
        Assert.IsAssignableFrom<ISearchService>(adapter);
    }

    [Fact]
    public void AzureSearchDocumentSearchAdapter_ImplementsISearchService()
    {
        // Act
        var adapter = new AzureSearchDocumentSearchAdapter();

        // Assert
        Assert.IsAssignableFrom<ISearchService>(adapter);
    }
}
