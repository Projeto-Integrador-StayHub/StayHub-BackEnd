using System.ComponentModel.DataAnnotations;

namespace StayHub_BackEnd.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
