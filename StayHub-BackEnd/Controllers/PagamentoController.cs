using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.Enums;
using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using Microsoft.Extensions.Logging;

namespace StayHub_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IConfiguration configuration, AppDbContext context, ILogger<PaymentController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("create-payment-intent/{reservaId}")]
        public IActionResult CreatePaymentIntent(int reservaId)
        {
            _logger.LogInformation($"Iniciando criação do PaymentIntent para a reserva ID: {reservaId}.");

            var reserva = _context.Reservas
                .FirstOrDefault(r => r.Id == reservaId);

            if (reserva == null)
            {
                _logger.LogWarning($"Reserva ID: {reservaId} não encontrada.");
                return BadRequest(new { Error = "Reserva inválida ou já processada." });
            }

            if (reserva.Status != ReservaStatus.Pendente)
            {
                _logger.LogWarning($"Reserva ID: {reservaId} não está com status pendente (status atual: {reserva.Status}).");
                return BadRequest(new { Error = "Reserva inválida ou já processada." });
            }

            if (reserva.Hospede == null)
            {
                _logger.LogWarning($"Hospede da reserva ID: {reservaId} não encontrado.");
                return BadRequest(new { Error = "Hospede não encontrado para a reserva." });
            }

            if (reserva.Status != ReservaStatus.Pendente)
            {
                _logger.LogWarning($"Reserva ID: {reservaId} não está com status pendente (status atual: {reserva.Status}).");
                return BadRequest(new { Error = "Reserva inválida ou já processada." });
            }

            try
            {
                _logger.LogInformation($"Criando PaymentIntent para a reserva ID: {reservaId}, valor: {reserva.Preco} BRL.");

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(reserva.Preco * 100), // Valor em centavos
                    Currency = "brl",
                    Metadata = new Dictionary<string, string>
                    {
                        { "ReservaId", reserva.Id.ToString() },
                        { "HospedeId", reserva.HospedeId.ToString() },
                        { "HospedeNome", reserva.Hospede.Nome }
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);

                _logger.LogInformation($"PaymentIntent criado com sucesso para a reserva ID: {reservaId}. ClientSecret: {paymentIntent.ClientSecret}");

                return Ok(new { ClientSecret = paymentIntent.ClientSecret });
            }
            catch (StripeException ex)
            {
                _logger.LogError($"Erro ao criar PaymentIntent para a reserva ID: {reservaId}. Mensagem do erro: {ex.Message}");
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public IActionResult StripeWebhook()
        {
            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookSecret"]
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var reservaId = int.Parse(paymentIntent.Metadata["ReservaId"]);

                    var reserva = _context.Reservas.FirstOrDefault(r => r.Id == reservaId);
                    if (reserva != null)
                    {
                        reserva.Status = ReservaStatus.Confirmada;
                        _context.SaveChanges();
                        Console.WriteLine($"Pagamento confirmado para reserva {reservaId}");
                    }
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine($"Falha no pagamento: {paymentIntent.LastPaymentError?.Message}");
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

    }
}
