using System.Net.Http.Json;

namespace EcoGestorApi.Tests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ReturnsHttpStatusCode200()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { email = "admin@ecogestor.com", senha = "123456" });
        response.EnsureSuccessStatusCode();
    }
}
