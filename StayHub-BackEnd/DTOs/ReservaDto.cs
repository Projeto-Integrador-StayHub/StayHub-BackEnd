using StayHub_BackEnd.Enums;

namespace StayHub_BackEnd.DTOs
{
    public class ReservaDto
    {
        public int HospedeId { get; set; }
        public int QuartoId { get; set; }
        public string Nome { get; set; } // Adicionado o campo Nome
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public decimal Preco { get; set; } // Adicionado o campo Preco
        public PagamentoStatu PagamentoStatus { get; set; } // Adicionado o campo PagamentoStatus
    }
}
