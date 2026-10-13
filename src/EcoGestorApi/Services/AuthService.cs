using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcoGestorApi.Data;
using EcoGestorApi.Models;
using Microsoft.AspNetCore.Identity;
using EcoGestorApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EcoGestorApi.Services;

public class AuthService
{
    private readonly EcoGestorDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(EcoGestorDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseViewModel?> LoginAsync(LoginViewModel login)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == login.Email);
        if (usuario is null) return null;
        if (new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.Senha, login.Senha) == PasswordVerificationResult.Failed) return null;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Perfil)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new LoginResponseViewModel(new JwtSecurityTokenHandler().WriteToken(token), usuario.Nome, usuario.Perfil);
    }
}
