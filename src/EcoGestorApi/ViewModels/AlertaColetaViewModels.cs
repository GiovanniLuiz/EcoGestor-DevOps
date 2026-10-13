namespace EcoGestorApi.ViewModels;

public record AlertaColetaResponseViewModel(
    int IdAlerta,
    int IdPonto,
    DateTime DataAlerta,
    string TipoAlerta,
    string Mensagem
);
