using StayHub_BackEnd.DTOs;

namespace StayHub_BackEnd.Services.Pagamentos
{
    public interface IPagamento
    {
        Task<string> CriarSessaoPagamento(PagamentoDto pagamentoDto);
    }
}
