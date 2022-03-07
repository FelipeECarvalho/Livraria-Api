using Livraria.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Livraria.ViewModels
{
    public class UpdateSaleViewModel
    {
        [Required(ErrorMessage = "O campo status é obrigatório")]
        public Status Status { get; set; }
        [Required(ErrorMessage = "O campo id livros é obrigatório")]
        public List<int> BooksId { get; set; }
    }
}
