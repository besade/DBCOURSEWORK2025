using System.Net;
using Xunit;

namespace Shop.Tests.Integration.Controllers;

public class CartControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CartControllerTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    [Fact]
    public async Task Index_Unauthenticated_RedirectsToLogin()
    {
        var client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        { AllowAutoRedirect = false });
        var response = await client.GetAsync("/Cart/Index");

        Assert.True(
        response.StatusCode == HttpStatusCode.Redirect ||
        response.StatusCode == HttpStatusCode.Found ||
        response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Index_Authenticated_Returns200()
    {
        var client = _factory.CreateClient();
        var token = AuthHelpers.GenerateToken(1, "user@example.com", false);
        client.DefaultRequestHeaders.Add("Cookie", $"jwt-token={token}");

        var response = await client.GetAsync("/Cart/Index");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}