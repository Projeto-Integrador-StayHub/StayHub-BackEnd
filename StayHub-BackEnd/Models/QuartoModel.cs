﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace StayHub_BackEnd.Models
{
    public class QuartoModel
    {
        public int Id { get; set; }
        public DonoHotelModel Dono {  get; set; }
        public string NomeQuarto { get; set; }
        public string Descricao { get; set; }
        public decimal Preco {  get; set; }
        public int CapacidadePessoas { get; set; }
        public bool Disponibilidade {  get; set; }
        public List<string> Comodidades { get; set; }
        public string Endereco { get; set; }
        public int DonoId { get; internal set; }

        [JsonIgnore]
        public ICollection<AvaliacaoModel> Avaliacao { get; set; }
    }
}
