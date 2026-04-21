using ASE.Libraries;
using ASE.Libraries.Search;

namespace ASE.Libraries.Tests;

public class AzureSearchDocumentSearchAdapterTests
{
    private readonly AzureSearchDocumentSearchAdapter _adapter;

    public AzureSearchDocumentSearchAdapterTests()
    {
        _adapter = new AzureSearchDocumentSearchAdapter();
    }

    [Fact]
    public void AzureSearchDocumentSearchAdapter_ImplementsISearchService()
    {
        // Assert
        Assert.IsAssignableFrom<ISearchService>(_adapter);
    }

    [Fact]
    public void Search_ThrowsNotImplementedException()
    {
        // Arrange
        var query = "test query";

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _adapter.Search(query));
    }

    [Fact]
    public void Search_WithRecordCount_ThrowsNotImplementedException()
    {
        // Arrange
        var query = "test query";
        var recordCount = 5;

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _adapter.Search(query, recordCount));
    }

    [Fact]
    public void AzureSearchDocumentSearchAdapter_CanBeInstantiated()
    {
        // Act
        var adapter = new AzureSearchDocumentSearchAdapter();

        // Assert
        Assert.NotNull(adapter);
    }
}
