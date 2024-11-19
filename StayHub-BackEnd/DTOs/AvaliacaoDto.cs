using StayHub_BackEnd.Enums;
using StayHub_BackEnd.Models;

namespace StayHub_BackEnd.DTOs
{
    public class AvaliacaoDto
    {
        public string Descricao { get; set; }
        public NotaAvaliacao Avaliacao { get; set; }
        //public QuartoModel Quarto { get; set; }
        //public HospedeModel Hospede { get; set; }
    }
}
