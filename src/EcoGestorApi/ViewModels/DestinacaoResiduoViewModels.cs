using System.ComponentModel.DataAnnotations;

namespace EcoGestorApi.ViewModels;

public record DestinacaoResiduoCreateViewModel(
    [Required] int IdColeta,
    [Required, MaxLength(100)] string EmpresaDestino,
    [Required, MaxLength(100)] string FormaTratamento
);

public record DestinacaoResiduoResponseViewModel(
    int IdDestinacao,
    int IdColeta,
    DateTime DataDestinacao,
    string EmpresaDestino,
    string FormaTratamento
);
