using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StayHub_BackEnd.DTOs;
using StayHub_BackEnd.Enums;
using StayHub_BackEnd.Services.Reserva;

namespace StayHub_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly IReserva _reservaService;
        private readonly ILogger<PagamentoController> _logger;

        public PagamentoController(IReserva reservaService, ILogger<PagamentoController> logger)
        {
            _reservaService = reservaService;
            _logger = logger;
        }

        [HttpGet("confirmar-pagamento/{reservaId}")]
        public async Task<IActionResult> ConfirmarPagamento(int reservaId)
        {
            _logger.LogInformation($"Confirmando pagamento para a reserva {reservaId}");
            var resposta = await _reservaService.BuscarReserva(reservaId);

            if (!resposta.Status || resposta.Dados == null)
            {
                _logger.LogWarning($"Reserva {reservaId} não encontrada: {resposta.Mensagem}");
                return BadRequest(new { Mensagem = resposta.Mensagem });
            }

            var reserva = resposta.Dados;
            double precoTotal = CalcularPrecoTotal(reserva.Entrada, reserva.Saida, (double)reserva.Preco);

            return Ok(new { ValorTotal = precoTotal, DetalhesReserva = reserva });
        }

        [HttpPost("processar-pagamento/{idReserva}")]
        public IActionResult ProcessarPagamento(int idReserva, string metodoPagamento, ReservaDto reservaDto)
        {
            _logger.LogInformation($"Processando pagamento para a reserva {idReserva} com o método {metodoPagamento}");
            var resposta = _reservaService.BuscarReserva(idReserva).Result;

            if (!resposta.Status || resposta.Dados == null)
            {
                _logger.LogWarning($"Reserva {idReserva} não encontrada: {resposta.Mensagem}");
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

                _logger.LogInformation($"Pagamento aprovado para a reserva {idReserva}");
                return Ok(new { Mensagem = "Pagamento aprovado! Sua reserva foi confirmada.", ValorTotal = precoTotal });
            }
            else
            {
                reserva.PagamentoStatus = PagamentoStatu.Falhou;
                reservaDto.PagamentoStatus = PagamentoStatu.Falhou;
                _reservaService.EditarReserva(idReserva, reservaDto);

                _logger.LogError($"Erro no pagamento para a reserva {idReserva}");
                return BadRequest(new { Mensagem = "Ocorreu um erro no pagamento. Tente novamente.", ValorTotal = precoTotal });
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
            // Simulate different payment methods
            if (string.IsNullOrEmpty(metodoPagamento))
            {
                return false;
            }

            switch (metodoPagamento.ToLower())
            {
                case "cartao":
                case "paypal":
                case "pix":
                    return true;
                default:
                    return false;
            }
        }
    }
}
