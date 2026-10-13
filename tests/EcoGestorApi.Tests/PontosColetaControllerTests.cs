namespace EcoGestorApi.Tests;

public class PontosColetaControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PontosColetaControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("/api/pontos-coleta");
        response.EnsureSuccessStatusCode();
    }
}
