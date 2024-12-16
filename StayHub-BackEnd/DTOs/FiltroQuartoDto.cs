namespace StayHub_BackEnd.DTOs
{
    public class FiltroQuartoDto
    {
        public string? NomeQuarto { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
        public int? CapacidadePessoas { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
    }
}
