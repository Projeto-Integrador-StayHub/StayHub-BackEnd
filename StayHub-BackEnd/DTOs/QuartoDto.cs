using StayHub_BackEnd.Models;
using System.Text.Json.Serialization;

namespace StayHub_BackEnd.DTOs
{
    public class QuartoDto
    {
        public string NomeQuarto { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int CapacidadePessoas { get; set; }
        public bool Disponibilidade { get; set; }
        public string Comodidades { get; set; }
        public string Endereco { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public int DonoId { get; set; }
    }
}

