namespace EcoGestorApi.ViewModels;

public record RelatorioImpactoViewModel(
    int TotalUnidades,
    int TotalPontosColeta,
    int TotalColetas,
    decimal TotalKgColetados,
    int TotalAlertasCriticos,
    int TotalDestinacoesFinalizadas
);
