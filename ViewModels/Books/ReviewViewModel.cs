using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Books
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "O campo titulo é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo titulo precisa ser entre 3 e 80 caracteres")]
        public string Title { get; set; }
        [Required(ErrorMessage = "O campo corpo é obrigatório")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "O campo corpo precisa ser entre 10 e 2000 caracteres")]
        public string Body { get; set; }
        [Required(ErrorMessage = "O campo titulo é obrigatório")]
        [Range(0, 5, ErrorMessage = "O campo avaliação precisa ser entre 0 e 5")]
        public int Stars { get; set; }
        [Required(ErrorMessage = "O campo id usuário é obrigatório")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "O campo id livro é obrigatório")]
        public int BookId { get; set; }
    }
}
