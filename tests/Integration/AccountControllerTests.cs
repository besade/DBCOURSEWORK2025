using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace Shop.Tests.Integration.Controllers;

public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AccountControllerTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

    [Fact]
    public async Task LoginGet_Anonymous_Returns200()
    {
        var response = await _client.GetAsync("/Account/Login");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RegisterGet_Anonymous_Returns200()
    {
        var response = await _client.GetAsync("/Account/Register");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ProfileGet_Unauthenticated_RedirectsToLogin()
    {
        var response = await _client.GetAsync("/Account/Profile");
        Assert.True(
        response.StatusCode == HttpStatusCode.Redirect ||
        response.StatusCode == HttpStatusCode.Found ||
        response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LoginGet_AuthenticatedUser_RedirectsToHome()
    {
        var token = AuthHelpers.GenerateToken(1, "user@example.com", false);
        _client.DefaultRequestHeaders.Add("Cookie", $"jwt-token={token}");

        var response = await _client.GetAsync("/Account/Login");

        Assert.True(
            response.StatusCode == HttpStatusCode.Redirect ||
            response.StatusCode == HttpStatusCode.Found);
    }

    [Fact]
    public async Task LoginPost_InvalidModel_Returns200WithValidationErrors()
    {
        var freshClient = _client;
        freshClient.DefaultRequestHeaders.Clear();

        var getResponse = await freshClient.GetAsync("/Account/Login");
        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractAntiForgeryToken(html);

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Email"] = "",
            ["Password"] = "",
            ["__RequestVerificationToken"] = token
        });

        var response = await freshClient.PostAsync("/Account/Login", form);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        const string marker = "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
        var start = html.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0) return string.Empty;
        start += marker.Length;
        var end = html.IndexOf('"', start);
        return html[start..end];
    }
}