using EcoGestorApi.Data;
using EcoGestorApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcoGestorApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _database = Guid.NewGuid().ToString();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "TestOnlySigningKey_32BytesMinimum_2026!",
            ["Database:MigrateOnStartup"] = "false"
        }));
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<EcoGestorDbContext>(options => options.UseInMemoryDatabase(_database));
        });
    }
    protected override Microsoft.Extensions.Hosting.IHost CreateHost(Microsoft.Extensions.Hosting.IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EcoGestorDbContext>();
        db.Database.EnsureCreated();
        var user = new Usuario { Nome = "Teste", Email = "admin@ecogestor.com", Perfil = "ADMIN" };
        user.Senha = new PasswordHasher<Usuario>().HashPassword(user, "123456");
        db.Usuarios.Add(user);
        db.SaveChanges();
        return host;
    }
}
