using EcoGestorApi.Data;
using EcoGestorApi.Helpers;
using EcoGestorApi.Services;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/destinacoes")]
public class DestinacoesResiduoController : ControllerBase
{
    private readonly EcoGestorDbContext _context;
    private readonly DestinacaoResiduoService _service;

    public DestinacoesResiduoController(EcoGestorDbContext context, DestinacaoResiduoService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<DestinacaoResiduoResponseViewModel>>> Get(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.DestinacoesResiduo.AsNoTracking().OrderBy(d => d.IdDestinacao)
            .Select(d => new DestinacaoResiduoResponseViewModel(d.IdDestinacao, d.IdColeta, d.DataDestinacao, d.EmpresaDestino, d.FormaTratamento));
        return Ok(await query.ToPagedResultAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DestinacaoResiduoResponseViewModel>> GetById(int id)
    {
        var d = await _context.DestinacoesResiduo.AsNoTracking().FirstOrDefaultAsync(x => x.IdDestinacao == id);
        if (d is null) return NotFound();
        return Ok(new DestinacaoResiduoResponseViewModel(d.IdDestinacao, d.IdColeta, d.DataDestinacao, d.EmpresaDestino, d.FormaTratamento));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<DestinacaoResiduoResponseViewModel>> Post(DestinacaoResiduoCreateViewModel model)
    {
        var destinacao = await _service.RegistrarDestinacaoAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = destinacao.IdDestinacao }, new DestinacaoResiduoResponseViewModel(destinacao.IdDestinacao, destinacao.IdColeta, destinacao.DataDestinacao, destinacao.EmpresaDestino, destinacao.FormaTratamento));
    }
}
