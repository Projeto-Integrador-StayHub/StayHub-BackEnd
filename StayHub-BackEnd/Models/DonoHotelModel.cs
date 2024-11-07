using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StayHub_BackEnd.Models
{
    public class DonoHotelModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public DateTime Nascimento { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter exatamente 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
        public string Cpf { get; set; }

        public string Endereco { get; set; }

        [JsonIgnore]
        public ICollection<QuartoModel> Quartos { get; set; }
    }
}
