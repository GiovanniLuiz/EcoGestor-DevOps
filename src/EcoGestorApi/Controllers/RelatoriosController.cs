using EcoGestorApi.Services;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/relatorios")]
[Authorize]
public class RelatoriosController : ControllerBase
{
    private readonly RelatorioService _service;

    public RelatoriosController(RelatorioService service)
    {
        _service = service;
    }

    [HttpGet("impacto")]
    public async Task<ActionResult<RelatorioImpactoViewModel>> Impacto()
    {
        return Ok(await _service.GerarImpactoAsync());
    }
}
