using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Books
{
    public class CreateBookViewModel
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
        [StringLength(255, MinimumLength = 3, ErrorMessage = "O campo image precisa ser entre 3 e 255 caracteres")]
        public string Image { get; set; }

        [Required(ErrorMessage = "O campo data de criação é obrigatório")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "O campo resumo é obrigatório")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "O número de páginas é obrigatório")]
        public int PagesNumber { get; set; }
        [Required(ErrorMessage = "O campo id autor é obrigatório")]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "O campo id categoria é obrigatório")]
        public int CategoryId { get; set; }
    }
}
