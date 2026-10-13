using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoGestorApi.Models;

[Table("DESTINACAO_RESIDUO")]
public class DestinacaoResiduo
{
    [Key]
    [Column("ID_DESTINACAO")]
    public int IdDestinacao { get; set; }

    [Required]
    [Column("ID_COLETA")]
    public int IdColeta { get; set; }

    [Column("DATA_DESTINACAO")]
    public DateTime DataDestinacao { get; set; } = DateTime.Now;

    [Required, MaxLength(100)]
    [Column("EMPRESA_DESTINO")]
    public string EmpresaDestino { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Column("FORMA_TRATAMENTO")]
    public string FormaTratamento { get; set; } = string.Empty;

    public ColetaResiduo? ColetaResiduo { get; set; }
}
