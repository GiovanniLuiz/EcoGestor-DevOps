using EcoGestorApi.Data;
using EcoGestorApi.Helpers;
using EcoGestorApi.Services;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/alertas")]
[Authorize]
public class AlertasColetaController : ControllerBase
{
    private readonly EcoGestorDbContext _context;
    private readonly AlertaColetaService _service;

    public AlertasColetaController(EcoGestorDbContext context, AlertaColetaService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AlertaColetaResponseViewModel>>> Get(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.AlertasColeta.AsNoTracking().OrderByDescending(a => a.DataAlerta)
            .Select(a => new AlertaColetaResponseViewModel(a.IdAlerta, a.IdPonto, a.DataAlerta, a.TipoAlerta, a.Mensagem));
        return Ok(await query.ToPagedResultAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AlertaColetaResponseViewModel>> GetById(int id)
    {
        var a = await _context.AlertasColeta.AsNoTracking().FirstOrDefaultAsync(x => x.IdAlerta == id);
        if (a is null) return NotFound();
        return Ok(new AlertaColetaResponseViewModel(a.IdAlerta, a.IdPonto, a.DataAlerta, a.TipoAlerta, a.Mensagem));
    }

    [HttpPost("verificar")]
    public async Task<ActionResult<IEnumerable<AlertaColetaResponseViewModel>>> Verificar()
    {
        var alertas = await _service.VerificarPontosCriticosAsync();
        return Ok(alertas.Select(a => new AlertaColetaResponseViewModel(a.IdAlerta, a.IdPonto, a.DataAlerta, a.TipoAlerta, a.Mensagem)));
    }
}
