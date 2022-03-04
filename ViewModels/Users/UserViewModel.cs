using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Users
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo nome precisa ser entre 3 e 80 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo email é obrigatório")]
        [StringLength(160, MinimumLength = 3, ErrorMessage = "O campo email precisa ser entre 3 e 160 caracteres")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo telefone é obrigatório")]
        [StringLength(30, MinimumLength = 10, ErrorMessage = "O campo telefone precisa ser entre 10 e 30 caracteres")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "O campo senha é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "O campo senha precisa ser entre 3 e 100 caracteres")]
        public string PasswordHash { get; set; }
    }
}
