using EcoGestorApi.Services;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseViewModel>> Login(LoginViewModel model)
    {
        var result = await _authService.LoginAsync(model);
        if (result is null) return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        return Ok(result);
    }
}
