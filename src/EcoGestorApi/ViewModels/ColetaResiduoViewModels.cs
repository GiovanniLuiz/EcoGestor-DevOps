using System.ComponentModel.DataAnnotations;

namespace EcoGestorApi.ViewModels;

public record ColetaResiduoCreateViewModel(
    [Required] int IdPonto,
    [Range(0.01, double.MaxValue)] decimal QuantidadeKg,
    [Required, MaxLength(100)] string DestinoPrevisto
);

public record ColetaResiduoResponseViewModel(
    int IdColeta,
    int IdPonto,
    DateTime DataColeta,
    decimal QuantidadeKg,
    string DestinoPrevisto,
    string StatusColeta
);
