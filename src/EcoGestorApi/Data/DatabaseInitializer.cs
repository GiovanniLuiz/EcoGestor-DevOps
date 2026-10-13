using EcoGestorApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(EcoGestorDbContext db, IConfiguration config)
    {
        await db.Database.MigrateAsync();
        if (!await db.Usuarios.AnyAsync())
        {
            var password = config["Bootstrap:Password"];
            if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
                throw new InvalidOperationException("Configure Bootstrap:Password com pelo menos 12 caracteres.");
            var user = new Usuario { Nome = "Administrador EcoGestor", Email = config["Bootstrap:Email"] ?? "admin@ecogestor.example", Perfil = "ADMIN" };
            user.Senha = new PasswordHasher<Usuario>().HashPassword(user, password);
            db.Usuarios.Add(user);
            await db.SaveChangesAsync();
        }
    }
}
