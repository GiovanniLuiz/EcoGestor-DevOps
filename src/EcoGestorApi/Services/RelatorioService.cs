using EcoGestorApi.Data;
using EcoGestorApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Services;

public class RelatorioService
{
    private readonly EcoGestorDbContext _context;

    public RelatorioService(EcoGestorDbContext context)
    {
        _context = context;
    }

    public async Task<RelatorioImpactoViewModel> GerarImpactoAsync()
    {
        return new RelatorioImpactoViewModel(
            await _context.Unidades.CountAsync(),
            await _context.PontosColeta.CountAsync(),
            await _context.ColetasResiduo.CountAsync(),
            await _context.ColetasResiduo.SumAsync(c => (decimal?)c.QuantidadeKg) ?? 0,
            await _context.AlertasColeta.CountAsync(a => a.TipoAlerta == "LIMITE_CRITICO"),
            await _context.DestinacoesResiduo.CountAsync()
        );
    }
}
