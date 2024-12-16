using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;

[ApiController]
[Route("filtro/[controller]")]
public class FiltroQuartoController : ControllerBase
{
    private readonly AppDbContext _context;

    public FiltroQuartoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuartos([FromQuery] FiltroQuartoDto filtro)
    {
        var query = _context.Quartos.AsQueryable();

        if (!string.IsNullOrEmpty(filtro.NomeQuarto))
        {
            query = query.Where(q => q.NomeQuarto.Contains(filtro.NomeQuarto));
        }

        if (filtro.PrecoMinimo.HasValue)
        {
            query = query.Where(q => q.Preco >= filtro.PrecoMinimo.Value);
        }

        if (filtro.PrecoMaximo.HasValue)
        {
            query = query.Where(q => q.Preco <= filtro.PrecoMaximo.Value);
        }

        if (filtro.CapacidadePessoas.HasValue)
        {
            query = query.Where(q => q.CapacidadePessoas >= filtro.CapacidadePessoas.Value);
        }

        if (!string.IsNullOrEmpty(filtro.Cidade))
        {
            query = query.Where(q => q.Cidade.Contains(filtro.Cidade));
        }

        if (!string.IsNullOrEmpty(filtro.Estado))
        {
            query = query.Where(q => q.Estado.Contains(filtro.Estado));
        }

        var quartosFiltrados = await query.ToListAsync();
        return Ok(quartosFiltrados);
    }
}
