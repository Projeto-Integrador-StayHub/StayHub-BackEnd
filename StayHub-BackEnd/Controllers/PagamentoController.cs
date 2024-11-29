using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Services.Pagamentos;

namespace StayHub_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamento _pagamentoInterface;
        public PagamentoController(IPagamento iIPagamento)
        {
            _pagamentoInterface = iIPagamento;
        }

        [HttpPost("CriarSessaoPagamento")]
        public async Task<IActionResult> CriarSessaoPagamento([FromBody] PagamentoDto pagamentoDto)
        {
            try
            {
                var sessionUrl = await _pagamentoInterface.CriarSessaoPagamento(pagamentoDto);
                return Ok(new { url = sessionUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

    }
}
