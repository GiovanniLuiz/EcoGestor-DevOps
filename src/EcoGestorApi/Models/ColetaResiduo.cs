using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoGestorApi.Models;

[Table("COLETA_RESIDUO")]
public class ColetaResiduo
{
    [Key]
    [Column("ID_COLETA")]
    public int IdColeta { get; set; }

    [Required]
    [Column("ID_PONTO")]
    public int IdPonto { get; set; }

    [Column("DATA_COLETA")]
    public DateTime DataColeta { get; set; } = DateTime.Now;

    [Range(0.01, double.MaxValue)]
    [Column("QUANTIDADE_KG", TypeName = "decimal(10,2)")]
    public decimal QuantidadeKg { get; set; }

    [Required, MaxLength(100)]
    [Column("DESTINO_PREVISTO")]
    public string DestinoPrevisto { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    [Column("STATUS_COLETA")]
    public string StatusColeta { get; set; } = "PENDENTE";

    public PontoColeta? PontoColeta { get; set; }
    public DestinacaoResiduo? DestinacaoResiduo { get; set; }
}
