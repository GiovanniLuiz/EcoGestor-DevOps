using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EcoGestorApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EcoGestorApi.Tests;

public class DevOpsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    public DevOpsTests(CustomWebApplicationFactory factory) { _client = factory.CreateClient(); }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_Returns401()
    {
        Assert.Equal(HttpStatusCode.Unauthorized, (await _client.GetAsync("/api/alertas")).StatusCode);
    }
    [Fact]
    public async Task WrongPassword_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { email = "admin@ecogestor.com", senha = "errada" });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    [Fact]
    public async Task Readiness_ReportsEnvironment()
    {
        var response = await _client.GetAsync("/health/ready");
        response.EnsureSuccessStatusCode();
        Assert.Equal("Testing", JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("environment").GetString());
    }
    [Fact]
    public async Task Unit_CanBeCreatedUpdatedAndDeleted()
    {
        var login = await _client.PostAsJsonAsync("/api/auth/login", new { email = "admin@ecogestor.com", senha = "123456" });
        login.EnsureSuccessStatusCode();
        var token = JsonDocument.Parse(await login.Content.ReadAsStringAsync()).RootElement.GetProperty("token").GetString();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var created = await _client.PostAsJsonAsync("/api/unidades", new { nomeUnidade = "Teste CRUD", cidade = "São Paulo", responsavel = "QA", metaReciclagemKg = 200 });
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var location = created.Headers.Location!;
        var updated = await _client.PutAsJsonAsync(location, new { nomeUnidade = "Atualizada", cidade = "São Paulo", responsavel = "QA", metaReciclagemKg = 300 });
        Assert.Equal(HttpStatusCode.NoContent, updated.StatusCode);
        var read = await _client.GetAsync(location);
        Assert.Equal(300, JsonDocument.Parse(await read.Content.ReadAsStringAsync()).RootElement.GetProperty("metaReciclagemKg").GetDecimal());
        Assert.Equal(HttpStatusCode.NoContent, (await _client.DeleteAsync(location)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync(location)).StatusCode);
    }
    [Fact]
    public void SqlServerMigration_IsDiscoverableAndGeneratesTables()
    {
        using var db = new EcoGestorDbContext(new DbContextOptionsBuilder<EcoGestorDbContext>().UseSqlServer("Server=unused;Database=unused").Options);
        Assert.Single(db.Database.GetMigrations());
        var sql = db.GetService<IMigrator>().GenerateScript();
        Assert.Contains("CREATE TABLE [UNIDADE]", sql);
        Assert.Contains("CREATE TABLE [USUARIO]", sql);
        Assert.DoesNotContain("123456", sql);
    }
}
