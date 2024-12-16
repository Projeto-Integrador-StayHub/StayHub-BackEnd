using Microsoft.EntityFrameworkCore;
using StayHub_BackEnd.Data;
using StayHub_BackEnd.DTOs;
using Stripe.Checkout;

namespace StayHub_BackEnd.Services.Pagamentos
{
    public class PagamentoService : IPagamento
    {
        private readonly AppDbContext _context;
        public PagamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> CriarSessaoPagamento(PagamentoDto pagamentoDto)
        {
            // Verificar se a reserva existe
            var reserva = await _context.Reservas
                .Include(r => r.Hospede)
                .FirstOrDefaultAsync(r => r.Id == pagamentoDto.ReservaId);

            if (reserva == null)
            {
                throw new Exception("Reserva não encontrada.");
            }

            // Criação da sessão de pagamento
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = pagamentoDto.Pagadores.Select(pagador => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "brl",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Pagamento para Reserva #{reserva.Id} - Quarto: {reserva.Nome}",
                            Description = $"Divisão de pagamento para {pagador.Email}"
                        },
                        UnitAmountDecimal = pagador.Preco * 100,
                    },
                    Quantity = 1
                }).ToList(),
                Mode = "payment",
                SuccessUrl = "https://www.youtube.com/watch?v=tRsQiCuGhnc",
                CancelUrl = "https://www.youtube.com/watch?v=Sc6dnE-Mir4",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
