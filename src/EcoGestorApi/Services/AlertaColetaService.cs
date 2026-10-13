using EcoGestorApi.Data;
using EcoGestorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Services;

public class AlertaColetaService
{
    private readonly EcoGestorDbContext _context;

    public AlertaColetaService(EcoGestorDbContext context)
    {
        _context = context;
    }

    public async Task<List<AlertaColeta>> VerificarPontosCriticosAsync()
    {
        var pontosCriticos = await _context.PontosColeta
            .Where(p => p.CapacidadeMaxKg > 0 && p.NivelAtualKg >= p.CapacidadeMaxKg * 0.8m)
            .ToListAsync();

        var alertasCriados = new List<AlertaColeta>();

        foreach (var ponto in pontosCriticos)
        {
            var existeAlertaHoje = await _context.AlertasColeta.AnyAsync(a =>
                a.IdPonto == ponto.IdPonto && a.DataAlerta.Date == DateTime.Today && a.TipoAlerta == "LIMITE_CRITICO");

            if (existeAlertaHoje) continue;

            var alerta = new AlertaColeta
            {
                IdPonto = ponto.IdPonto,
                DataAlerta = DateTime.Now,
                TipoAlerta = "LIMITE_CRITICO",
                Mensagem = $"Ponto de coleta de {ponto.TipoResiduo} acima de 80% da capacidade."
            };

            _context.AlertasColeta.Add(alerta);
            alertasCriados.Add(alerta);
        }

        await _context.SaveChangesAsync();
        return alertasCriados;
    }
}
