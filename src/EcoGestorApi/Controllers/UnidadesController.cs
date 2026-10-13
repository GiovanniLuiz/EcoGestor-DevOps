using EcoGestorApi.Data;
using EcoGestorApi.Helpers;
using EcoGestorApi.Models;
using EcoGestorApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Controllers;

[ApiController]
[Route("api/unidades")]
public class UnidadesController : ControllerBase
{
    private readonly EcoGestorDbContext _context;

    public UnidadesController(EcoGestorDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UnidadeResponseViewModel>>> Get(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Unidades.AsNoTracking()
            .OrderBy(u => u.IdUnidade)
            .Select(u => new UnidadeResponseViewModel(u.IdUnidade, u.NomeUnidade, u.Cidade, u.Responsavel, u.MetaReciclagemKg));

        return Ok(await query.ToPagedResultAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UnidadeResponseViewModel>> GetById(int id)
    {
        var unidade = await _context.Unidades.AsNoTracking().FirstOrDefaultAsync(u => u.IdUnidade == id);
        if (unidade is null) return NotFound();
        return Ok(new UnidadeResponseViewModel(unidade.IdUnidade, unidade.NomeUnidade, unidade.Cidade, unidade.Responsavel, unidade.MetaReciclagemKg));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<ActionResult<UnidadeResponseViewModel>> Post(UnidadeCreateViewModel model)
    {
        var unidade = new Unidade { NomeUnidade = model.NomeUnidade, Cidade = model.Cidade, Responsavel = model.Responsavel, MetaReciclagemKg = model.MetaReciclagemKg };
        _context.Unidades.Add(unidade);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = unidade.IdUnidade }, new UnidadeResponseViewModel(unidade.IdUnidade, unidade.NomeUnidade, unidade.Cidade, unidade.Responsavel, unidade.MetaReciclagemKg));
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UnidadeCreateViewModel model)
    {
        var unidade = await _context.Unidades.FindAsync(id);
        if (unidade is null) return NotFound();
        unidade.NomeUnidade = model.NomeUnidade;
        unidade.Cidade = model.Cidade;
        unidade.Responsavel = model.Responsavel;
        unidade.MetaReciclagemKg = model.MetaReciclagemKg;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var unidade = await _context.Unidades.FindAsync(id);
        if (unidade is null) return NotFound();
        _context.Unidades.Remove(unidade);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
