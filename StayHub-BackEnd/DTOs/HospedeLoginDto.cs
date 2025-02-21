// Updated UsuarioLoginDto.cs to align with the modified fields if needed.
// Typically, login only requires email and senha, so you might keep it minimal.
using System.ComponentModel.DataAnnotations;

namespace StayHub_BackEnd.Dtos
{
    public class HospedeLoginDto
    {
        [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatória")]
        public string Senha { get; set; }
    }
}
