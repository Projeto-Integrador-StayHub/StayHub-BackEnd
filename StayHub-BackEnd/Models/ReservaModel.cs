using StayHub_BackEnd.Enums;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.Models
{
    public class ReservaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } // Adicionado o campo Nome
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public decimal Preco { get; set; } // Adicionado o campo Preco
        public ReservaStatus Status { get; set; } = ReservaStatus.Confirmada; // Define o status como Confirmada por padrão
        public PagamentoStatu PagamentoStatus { get; set; } // Adicionado o campo PagamentoStatus
        public HospedeModel Hospede { get; set; }
        public int HospedeId { get; set; }
        public int QuartoId { get; set; } // Adicionado o campo QuartoId
    }
}

