using ASE.Libraries.General;

namespace ASE.Libraries.Tests;

public class RouteNamesTests
{
    [Fact]
    public void BasicRoute_HasCorrectValue()
    {
        // Assert
        Assert.Equal("basic", RouteNames.BasicRoute);
    }

    [Fact]
    public void GetRoute_HasCorrectValue()
    {
        // Assert
        Assert.Equal("get-all", RouteNames.GetRoute);
    }

    [Fact]
    public void SearchRoute_HasCorrectValue()
    {
        // Assert
        Assert.Equal("search", RouteNames.SearchRoute);
    }

    [Fact]
    public void RouteNames_ConstantsAreNotNull()
    {
        // Assert
        Assert.NotNull(RouteNames.BasicRoute);
        Assert.NotNull(RouteNames.GetRoute);
        Assert.NotNull(RouteNames.SearchRoute);
    }

    [Fact]
    public void RouteNames_ConstantsAreNotEmpty()
    {
        // Assert
        Assert.NotEmpty(RouteNames.BasicRoute);
        Assert.NotEmpty(RouteNames.GetRoute);
        Assert.NotEmpty(RouteNames.SearchRoute);
    }

    [Fact]
    public void RouteNames_AllConstantsAreLowercase()
    {
        // Assert
        Assert.Equal(RouteNames.BasicRoute, RouteNames.BasicRoute.ToLower());
        Assert.Equal(RouteNames.GetRoute, RouteNames.GetRoute.ToLower());
        Assert.Equal(RouteNames.SearchRoute, RouteNames.SearchRoute.ToLower());
    }

    [Fact]
    public void RouteNames_ConstantsAreUnique()
    {
        // Arrange
        var routes = new[] { RouteNames.BasicRoute, RouteNames.GetRoute, RouteNames.SearchRoute };

        // Assert
        Assert.Equal(routes.Length, routes.Distinct().Count());
    }
}
