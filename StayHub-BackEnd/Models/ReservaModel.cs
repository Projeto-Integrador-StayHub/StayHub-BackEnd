using StayHub_BackEnd.Enums;

namespace StayHub_BackEnd.Models
{
    public class ReservaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public decimal Preco { get; set; }
        public ReservaStatus Status { get; set; }
        public HospedeModel Hospede { get; set; }
        public int HospedeId { get; set; }
        public PagamentoStatu PagamentoStatus { get; set; }
    }
}
