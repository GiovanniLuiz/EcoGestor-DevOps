using EcoGestorApi.Data;
using EcoGestorApi.Helpers;
using EcoGestorApi.Services;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/coletas")]
public class ColetasResiduoController : ControllerBase
{
    private readonly EcoGestorDbContext _context;
    private readonly ColetaResiduoService _service;

    public ColetasResiduoController(EcoGestorDbContext context, ColetaResiduoService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ColetaResiduoResponseViewModel>>> Get(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.ColetasResiduo.AsNoTracking().OrderBy(c => c.IdColeta)
            .Select(c => new ColetaResiduoResponseViewModel(c.IdColeta, c.IdPonto, c.DataColeta, c.QuantidadeKg, c.DestinoPrevisto, c.StatusColeta));
        return Ok(await query.ToPagedResultAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ColetaResiduoResponseViewModel>> GetById(int id)
    {
        var c = await _context.ColetasResiduo.AsNoTracking().FirstOrDefaultAsync(x => x.IdColeta == id);
        if (c is null) return NotFound();
        return Ok(new ColetaResiduoResponseViewModel(c.IdColeta, c.IdPonto, c.DataColeta, c.QuantidadeKg, c.DestinoPrevisto, c.StatusColeta));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ColetaResiduoResponseViewModel>> Post(ColetaResiduoCreateViewModel model)
    {
        var coleta = await _service.RegistrarColetaAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = coleta.IdColeta }, new ColetaResiduoResponseViewModel(coleta.IdColeta, coleta.IdPonto, coleta.DataColeta, coleta.QuantidadeKg, coleta.DestinoPrevisto, coleta.StatusColeta));
    }
}
