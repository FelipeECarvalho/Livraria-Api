using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels.Sales
{
    public class CreateSaleViewModel
    {
        [Required(ErrorMessage = "O campo data é obrigatório")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "O campo id usuário é obrigatório")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "O campo id livros é obrigatório")]
        public List<int> BooksId { get; set; }
    }
}
