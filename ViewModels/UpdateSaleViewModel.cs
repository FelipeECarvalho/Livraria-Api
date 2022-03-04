using Livraria.Models.Books;
using Livraria.Models.Enums;

namespace Livraria.ViewModels
{
    public class UpdateSaleViewModel
    {
        public Status Status { get; set; }
        public List<int> BooksId { get; set; }
    }
}
