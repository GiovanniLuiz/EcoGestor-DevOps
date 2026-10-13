using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoGestorApi.Models;

[Table("PONTO_COLETA")]
public class PontoColeta
{
    [Key]
    [Column("ID_PONTO")]
    public int IdPonto { get; set; }

    [Required]
    [Column("ID_UNIDADE")]
    public int IdUnidade { get; set; }

    [Required, MaxLength(50)]
    [Column("TIPO_RESIDUO")]
    public string TipoResiduo { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    [Column("CAPACIDADE_MAX_KG", TypeName = "decimal(10,2)")]
    public decimal CapacidadeMaxKg { get; set; }

    [Range(0, double.MaxValue)]
    [Column("NIVEL_ATUAL_KG", TypeName = "decimal(10,2)")]
    public decimal NivelAtualKg { get; set; }

    [Required, MaxLength(20)]
    [Column("STATUS_PONTO")]
    public string StatusPonto { get; set; } = "NORMAL";

    public Unidade? Unidade { get; set; }
    public ICollection<ColetaResiduo> ColetasResiduo { get; set; } = new List<ColetaResiduo>();
    public ICollection<AlertaColeta> AlertasColeta { get; set; } = new List<AlertaColeta>();
}
