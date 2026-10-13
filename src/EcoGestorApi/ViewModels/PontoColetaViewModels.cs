using System.ComponentModel.DataAnnotations;

namespace EcoGestorApi.ViewModels;

public record PontoColetaCreateViewModel(
    [Required] int IdUnidade,
    [Required, MaxLength(50)] string TipoResiduo,
    [Range(0.01, double.MaxValue)] decimal CapacidadeMaxKg,
    [Range(0, double.MaxValue)] decimal NivelAtualKg
);

public record PontoColetaResponseViewModel(
    int IdPonto,
    int IdUnidade,
    string? NomeUnidade,
    string TipoResiduo,
    decimal CapacidadeMaxKg,
    decimal NivelAtualKg,
    string StatusPonto
);
