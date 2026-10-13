namespace EcoGestorApi.Tests;

public class DestinacoesResiduoControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DestinacoesResiduoControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("/api/destinacoes");
        response.EnsureSuccessStatusCode();
    }
}
