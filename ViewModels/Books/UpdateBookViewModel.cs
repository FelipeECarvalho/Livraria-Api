using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Books
{
    public class UpdateBookViewModel
    {
        [Required(ErrorMessage = "O campo titulo é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo titulo precisa ser entre 3 e 80 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O campo preço é obrigatório")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "O campo idioma é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo idioma precisa ser entre 3 e 80 caracteres")]
        public string Language { get; set; }

        [Required(ErrorMessage = "O campo imagem é obrigatório")]
        public string Image { get; set; }

        [Required(ErrorMessage = "O campo data de criação é obrigatório")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "O campo resumo é obrigatório")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "O campo titulo precisa ser entre 10 e 2000 caracteres")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "O campo número de páginas é obrigatório")]
        public int PagesNumber { get; set; }
    }
}
