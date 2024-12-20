using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.DTOs
{
    public class PagamentoDto
    {
        public int ReservaId { get; set; }
        public List<PagamentoParteDto> Pagadores { get; set; }
    }

    public class PagamentoParteDto
    {
        public HospedeModel Email { get; set; }
        public decimal Preco { get; set; } //reservamodel preco
    }
}
