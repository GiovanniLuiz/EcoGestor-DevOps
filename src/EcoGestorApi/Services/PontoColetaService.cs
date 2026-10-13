using EcoGestorApi.Data;
using EcoGestorApi.Models;
using EcoGestorApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Services;

public class PontoColetaService
{
    private readonly EcoGestorDbContext _context;

    public PontoColetaService(EcoGestorDbContext context)
    {
        _context = context;
    }

    public string CalcularStatus(decimal nivelAtualKg, decimal capacidadeMaxKg)
    {
        if (capacidadeMaxKg <= 0) return "NORMAL";
        var ocupacao = nivelAtualKg / capacidadeMaxKg;
        if (ocupacao >= 0.8m) return "CRITICO";
        if (ocupacao >= 0.5m) return "ATENCAO";
        return "NORMAL";
    }

    public async Task<PontoColeta> CriarAsync(PontoColetaCreateViewModel model)
    {
        var unidadeExiste = await _context.Unidades.AnyAsync(u => u.IdUnidade == model.IdUnidade);
        if (!unidadeExiste) throw new InvalidOperationException("Unidade informada não existe.");
        if (model.NivelAtualKg > model.CapacidadeMaxKg) throw new InvalidOperationException("O nível atual não pode ser maior que a capacidade máxima.");

        var ponto = new PontoColeta
        {
            IdUnidade = model.IdUnidade,
            TipoResiduo = model.TipoResiduo.ToUpperInvariant(),
            CapacidadeMaxKg = model.CapacidadeMaxKg,
            NivelAtualKg = model.NivelAtualKg,
            StatusPonto = CalcularStatus(model.NivelAtualKg, model.CapacidadeMaxKg)
        };

        _context.PontosColeta.Add(ponto);
        await _context.SaveChangesAsync();
        return ponto;
    }
}
