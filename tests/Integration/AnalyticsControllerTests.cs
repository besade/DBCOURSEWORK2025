using System.Net;
using Xunit;

namespace Shop.Tests.Integration.Controllers;

public class AnalyticsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AnalyticsControllerTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    [Fact]
    public async Task Index_Unauthenticated_Redirects()
    {
        var client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        { AllowAutoRedirect = false });
        var response = await client.GetAsync("/Analytics/Index");

        Assert.True(
        response.StatusCode == HttpStatusCode.Redirect ||
        response.StatusCode == HttpStatusCode.Found ||
        response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Index_AsAdmin_Returns200()
    {
        var client = _factory.CreateClient();
        var token = AuthHelpers.GenerateToken(1, "admin@example.com", isAdmin: true);
        client.DefaultRequestHeaders.Add("Cookie", $"jwt-token={token}");

        var response = await client.GetAsync("/Analytics/Index");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Index_AsRegularUser_ForbiddenOrRedirect()
    {
        var client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        { AllowAutoRedirect = false });
        var token = AuthHelpers.GenerateToken(2, "user@example.com", isAdmin: false);
        client.DefaultRequestHeaders.Add("Cookie", $"jwt-token={token}");

        var response = await client.GetAsync("/Analytics/Index");
        Assert.True(
            response.StatusCode == HttpStatusCode.Forbidden ||
            response.StatusCode == HttpStatusCode.Redirect);
    }
}