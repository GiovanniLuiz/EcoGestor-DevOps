using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoGestorApi.Models;

[Table("ALERTA_COLETA")]
public class AlertaColeta
{
    [Key]
    [Column("ID_ALERTA")]
    public int IdAlerta { get; set; }

    [Required]
    [Column("ID_PONTO")]
    public int IdPonto { get; set; }

    [Column("DATA_ALERTA")]
    public DateTime DataAlerta { get; set; } = DateTime.Now;

    [Required, MaxLength(50)]
    [Column("TIPO_ALERTA")]
    public string TipoAlerta { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    [Column("MENSAGEM")]
    public string Mensagem { get; set; } = string.Empty;

    public PontoColeta? PontoColeta { get; set; }
}
