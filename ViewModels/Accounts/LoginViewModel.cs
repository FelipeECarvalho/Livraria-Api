using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        public string Password { get; set; }
    }
}
