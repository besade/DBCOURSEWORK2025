using System.Net;
using Xunit;

namespace Shop.Tests.Integration.Controllers;

public class HomeControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HomeControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Index_AnonymousUser_Returns200()
    {
        var response = await _client.GetAsync("/Home/Index");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Index_WithCategoryFilter_Returns200()
    {
        var response = await _client.GetAsync("/Home/Index?categoryId=1&page=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Index_PageTwo_Returns200()
    {
        var response = await _client.GetAsync("/Home/Index?page=2");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}