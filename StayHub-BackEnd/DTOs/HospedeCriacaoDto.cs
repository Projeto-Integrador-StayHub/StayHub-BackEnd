using System.ComponentModel.DataAnnotations;

namespace StayHub_BackEnd.Dtos
{
    public class HospedeCriacaoDto
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo telefone é obrigatório")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo nascimento é obrigatório")]
        public DateTime Nascimento { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter exatamente 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo endereço é obrigatório")]
        public string Endereco { get; set; }
    }
}
