using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Books
{
    public class AuthorViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo nome precisa ser entre 3 e 80 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo resumo é obrigatório")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "O campo resumo precisa ser entre 10 e 2000 caracteres")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "O campo foto é obrigatório")]
        public string Photo { get; set; }
    }
}
