using StayHub_BackEnd.Enums;
using StayHub_BackEnd.Models;
using StayHub_BackEnd.DTOs;

namespace StayHub_BackEnd.DTOs
{
    public class ReservaDto
    {
        public int HospedeId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public decimal Preco { get; set; }
        public ReservaStatus Status { get; set; }
    }
}
