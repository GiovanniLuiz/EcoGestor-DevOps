namespace EcoGestorApi.Tests;

public class ColetasResiduoControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ColetasResiduoControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("/api/coletas");
        response.EnsureSuccessStatusCode();
    }
}
