using Microsoft.AspNetCore.Mvc;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Enums;
using StayHub_BackEnd.Services.Reserva;

namespace StayHub_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private IReserva _reservaService;

        public PagamentoController(IReserva reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet("confirmar-pagamento/{reservaId}")]
        public async Task<IActionResult> ConfirmarPagamento(int reservaId)
        {
            var resposta = await _reservaService.BuscarReserva(reservaId);

            if (!resposta.Status || resposta.Dados == null)
            {
                return BadRequest(new { Mensagem = resposta.Mensagem });
            }

            var reserva = resposta.Dados;

            double precoTotal = CalcularPrecoTotal(reserva.Entrada, reserva.Saida, (double)reserva.Preco);

            return Ok(new { ValorTotal = precoTotal, DetalhesReserva = reserva });
        }

        [HttpPost("processar-pagamento/{idReserva}")]
        public IActionResult ProcessarPagamento(int idReserva, string metodoPagamento, ReservaDto reservaDto)
        {
            var resposta = _reservaService.BuscarReserva(idReserva).Result;

            if (!resposta.Status || resposta.Dados == null)
            {
                return BadRequest(new { Mensagem = resposta.Mensagem });
            }

            var reserva = resposta.Dados;

            double precoTotal = CalcularPrecoTotal(reservaDto.Entrada, reservaDto.Saida, (double)reservaDto.Preco);

            bool pagamentoAprovado = SimularPagamento(metodoPagamento);

            if (pagamentoAprovado)
            {
                reserva.PagamentoStatus = PagamentoStatu.Aprovado;
                reserva.Status = ReservaStatus.Confirmada;

                reservaDto.Preco = (decimal)precoTotal;
                _reservaService.EditarReserva(idReserva, reservaDto);

                return Ok(new { Mensagem = "Pagamento aprovado! Sua reserva foi confirmada." });
            }
            else
            {
                return BadRequest(new { Mensagem = "Ocorreu um erro no pagamento. Tente novamente." });
            }
        }

        private double CalcularPrecoTotal(DateTime dataEntrada, DateTime dataSaida, double precoDiario)
        {
            var diasDeEstadia = (dataSaida - dataEntrada).Days;
            if (diasDeEstadia <= 0) return 0;

            return diasDeEstadia * precoDiario;
        }

        private bool SimularPagamento(string metodoPagamento)
        {
            return !string.IsNullOrEmpty(metodoPagamento);
        }
    }
}
