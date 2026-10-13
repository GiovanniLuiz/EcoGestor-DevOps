using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EcoGestorApi.Tests;

public class AlertasColetaControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AlertasColetaControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHttpStatusCode200()
    {
        var login = await _client.PostAsJsonAsync("/api/auth/login", new { email = "admin@ecogestor.com", senha = "123456" });
        var json = await login.Content.ReadAsStringAsync();
        var token = JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/alertas");
        response.EnsureSuccessStatusCode();
    }
}
