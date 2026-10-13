using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoGestorApi.Models;

[Table("UNIDADE")]
public class Unidade
{
    [Key]
    [Column("ID_UNIDADE")]
    public int IdUnidade { get; set; }

    [Required, MaxLength(100)]
    [Column("NOME_UNIDADE")]
    public string NomeUnidade { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Column("CIDADE")]
    public string Cidade { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Column("RESPONSAVEL")]
    public string Responsavel { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    [Column("META_RECICLAGEM_KG", TypeName = "decimal(10,2)")]
    public decimal MetaReciclagemKg { get; set; }

    public ICollection<PontoColeta> PontosColeta { get; set; } = new List<PontoColeta>();
}
