using EcoGestorApi.Data;
using EcoGestorApi.Models;
using EcoGestorApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Services;

public class DestinacaoResiduoService
{
    private readonly EcoGestorDbContext _context;

    public DestinacaoResiduoService(EcoGestorDbContext context)
    {
        _context = context;
    }

    public async Task<DestinacaoResiduo> RegistrarDestinacaoAsync(DestinacaoResiduoCreateViewModel model)
    {
        var coleta = await _context.ColetasResiduo.FirstOrDefaultAsync(c => c.IdColeta == model.IdColeta);
        if (coleta is null) throw new InvalidOperationException("Coleta não encontrada.");

        var destinacaoJaExiste = await _context.DestinacoesResiduo.AnyAsync(d => d.IdColeta == model.IdColeta);
        if (destinacaoJaExiste) throw new InvalidOperationException("Essa coleta já possui destinação registrada.");

        var destinacao = new DestinacaoResiduo
        {
            IdColeta = model.IdColeta,
            DataDestinacao = DateTime.Now,
            EmpresaDestino = model.EmpresaDestino,
            FormaTratamento = model.FormaTratamento
        };

        coleta.StatusColeta = "FINALIZADA";
        _context.DestinacoesResiduo.Add(destinacao);
        await _context.SaveChangesAsync();
        return destinacao;
    }
}
