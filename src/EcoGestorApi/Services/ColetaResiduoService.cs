using EcoGestorApi.Data;
using EcoGestorApi.Models;
using EcoGestorApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Services;

public class ColetaResiduoService
{
    private readonly EcoGestorDbContext _context;
    private readonly PontoColetaService _pontoColetaService;
    private readonly AlertaColetaService _alertaColetaService;

    public ColetaResiduoService(EcoGestorDbContext context, PontoColetaService pontoColetaService, AlertaColetaService alertaColetaService)
    {
        _context = context;
        _pontoColetaService = pontoColetaService;
        _alertaColetaService = alertaColetaService;
    }

    public async Task<ColetaResiduo> RegistrarColetaAsync(ColetaResiduoCreateViewModel model)
    {
        var ponto = await _context.PontosColeta.FirstOrDefaultAsync(p => p.IdPonto == model.IdPonto);
        if (ponto is null) throw new InvalidOperationException("Ponto de coleta não encontrado.");
        if (model.QuantidadeKg > ponto.NivelAtualKg) throw new InvalidOperationException("A quantidade coletada não pode ser maior que o nível atual do ponto.");

        var coleta = new ColetaResiduo
        {
            IdPonto = model.IdPonto,
            DataColeta = DateTime.Now,
            QuantidadeKg = model.QuantidadeKg,
            DestinoPrevisto = model.DestinoPrevisto,
            StatusColeta = "PENDENTE"
        };

        ponto.NivelAtualKg -= model.QuantidadeKg;
        ponto.StatusPonto = _pontoColetaService.CalcularStatus(ponto.NivelAtualKg, ponto.CapacidadeMaxKg);

        _context.ColetasResiduo.Add(coleta);
        await _context.SaveChangesAsync();
        await _alertaColetaService.VerificarPontosCriticosAsync();
        return coleta;
    }
}
