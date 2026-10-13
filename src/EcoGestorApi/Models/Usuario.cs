using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoGestorApi.Models;

[Table("USUARIO")]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Required, MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    [Column("EMAIL")]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    [Column("SENHA")]
    public string Senha { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    [Column("PERFIL")]
    public string Perfil { get; set; } = "OPERADOR";
}
