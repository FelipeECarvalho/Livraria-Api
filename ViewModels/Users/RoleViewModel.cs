using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Users
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo nome precisa ser entre 3 e 80 caracteres")]
        public string Name { get; set; }
    }
}
