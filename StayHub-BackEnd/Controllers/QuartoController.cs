using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Services.Quarto;

namespace StayHub_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuartoController : ControllerBase
    {
        private readonly IQuarto _iquarto;
        public QuartoController(IQuarto iQuarto)
        {
            _iquarto = iQuarto;
        }

        [HttpGet("ListarQuartos")]
        public async Task<ActionResult<ResponseModel<List<QuartoModel>>>> ListarQuartos()
        {
            var quartos = await _iquarto.ListarQuartos();
            return Ok(quartos);
        }

        [HttpGet("BuscarQuartoId/{idQuarto}")]
        public async Task<ActionResult<ResponseModel<QuartoModel>>> BuscarQuarto(int idQuarto)
        {
            var quarto = await _iquarto.BuscarQuarto(idQuarto);
            return Ok(quarto);
        }

        [HttpPost("CriarQuarto")]
        public async Task<ActionResult<ResponseModel<QuartoModel>>> CriarQuarto(QuartoDto quartoDto)
        {
            var quartos = await _iquarto.CriarQuarto(quartoDto);
            return Ok(quartos);
        }

        [HttpPut("EditarQuarto/{idQuarto}")]
        public async Task<ActionResult<ResponseModel<QuartoModel>>> EditarQuarto(int idQuarto, QuartoDto quartoDto)
        {
            var quartos = await _iquarto.EditarQuarto(idQuarto, quartoDto);
            return Ok(quartos);
        }

        [HttpDelete("ExcluirQuarto/{idQuarto}")]
        public async Task<ActionResult<ResponseModel<QuartoModel>>> ExcluirQuarto(int idQuarto)
        {
            var quartos = await _iquarto.ExcluirQuarto(idQuarto);
            return Ok(quartos);
        }


    }
}
