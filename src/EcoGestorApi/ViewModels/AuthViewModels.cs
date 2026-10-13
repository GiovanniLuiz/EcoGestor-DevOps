using System.ComponentModel.DataAnnotations;

namespace EcoGestorApi.ViewModels;

public record LoginViewModel([Required, EmailAddress] string Email, [Required] string Senha);
public record LoginResponseViewModel(string Token, string Nome, string Perfil);
