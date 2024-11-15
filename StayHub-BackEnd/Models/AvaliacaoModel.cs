using StayHub_BackEnd.Enums;

namespace StayHub_BackEnd.Models
{
    public class AvaliacaoModel
    {
        public int Id { get; set; }
        public QuartoModel Quarto { get; set; }
        public HospedeModel Hospede { get; set; }
        public NotaAvaliacao Avaliacao { get; set; }
        public string Descricao { get; set; }
    }
}
