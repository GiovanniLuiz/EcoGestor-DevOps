using System.ComponentModel.DataAnnotations;

namespace EcoGestorApi.ViewModels;

public record UnidadeCreateViewModel(
    [Required, MaxLength(100)] string NomeUnidade,
    [Required, MaxLength(100)] string Cidade,
    [Required, MaxLength(100)] string Responsavel,
    [Range(0.01, double.MaxValue)] decimal MetaReciclagemKg
);

public record UnidadeResponseViewModel(int IdUnidade, string NomeUnidade, string Cidade, string Responsavel, decimal MetaReciclagemKg);
