using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.Services.Reserva;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReserva _reservaInterface;
        public ReservaController(IReserva iReserva)
        {
            _reservaInterface = iReserva;
        }

        [HttpGet("ListarReservas")]
        public async Task<ActionResult<ResponseModel<List<ReservaModel>>>> ListarReservas()
        {
            var reservas = await _reservaInterface.ListarReservas();
            return Ok(reservas);
        }

        [HttpGet("BuscarReserva/{idReserva}")]
        public async Task<ActionResult<ResponseModel<ReservaModel>>> BuscarReservas(int idReserva)
        {
            var reserva = await _reservaInterface.BuscarReserva(idReserva);
            return Ok(reserva);
        }

        [HttpPost("CriarReserva")]
        public async Task<IActionResult> CriarReserva([FromBody] ReservaDto reservaDto)
        {
            if (reservaDto == null)
            {
                return BadRequest("O campo reservaDto é obrigatório.");
            }

            var resultado = await _reservaInterface.CriarReserva(reservaDto);

            if (resultado.Status)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado.Mensagem);
            }
        }

        [HttpPut("EditarReserva/{idReserva}")]
        public async Task<ActionResult<ResponseModel<List<ReservaModel>>>> EditarReserva(int idReserva, ReservaDto reservaDto)
        {
            var reserva = await _reservaInterface.EditarReserva(idReserva, reservaDto);
            return Ok(reserva);
        }

        [HttpDelete("ExcluirReserva/{idReserva}")]
        public async Task<ActionResult<ResponseModel<List<ReservaModel>>>> ExcluirReserva(int idReserva)
        {
            var reserva = await _reservaInterface.ExcluirReserva(idReserva);
            return Ok(reserva);
        }
    }
}


