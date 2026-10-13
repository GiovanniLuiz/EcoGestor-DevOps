using EcoGestorApi.Data;
using EcoGestorApi.Helpers;
using EcoGestorApi.Services;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/pontos-coleta")]
public class PontosColetaController : ControllerBase
{
    private readonly EcoGestorDbContext _context;
    private readonly PontoColetaService _service;

    public PontosColetaController(EcoGestorDbContext context, PontoColetaService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PontoColetaResponseViewModel>>> Get(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.PontosColeta.AsNoTracking().Include(p => p.Unidade)
            .OrderBy(p => p.IdPonto)
            .Select(p => new PontoColetaResponseViewModel(p.IdPonto, p.IdUnidade, p.Unidade!.NomeUnidade, p.TipoResiduo, p.CapacidadeMaxKg, p.NivelAtualKg, p.StatusPonto));

        return Ok(await query.ToPagedResultAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PontoColetaResponseViewModel>> GetById(int id)
    {
        var ponto = await _context.PontosColeta.AsNoTracking().Include(p => p.Unidade).FirstOrDefaultAsync(p => p.IdPonto == id);
        if (ponto is null) return NotFound();
        return Ok(new PontoColetaResponseViewModel(ponto.IdPonto, ponto.IdUnidade, ponto.Unidade?.NomeUnidade, ponto.TipoResiduo, ponto.CapacidadeMaxKg, ponto.NivelAtualKg, ponto.StatusPonto));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PontoColetaResponseViewModel>> Post(PontoColetaCreateViewModel model)
    {
        var ponto = await _service.CriarAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = ponto.IdPonto }, new PontoColetaResponseViewModel(ponto.IdPonto, ponto.IdUnidade, null, ponto.TipoResiduo, ponto.CapacidadeMaxKg, ponto.NivelAtualKg, ponto.StatusPonto));
    }
}
