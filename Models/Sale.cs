using Livraria.Models.Books;
using Livraria.Models.Enums;
using Livraria.Models.Users;

namespace Livraria.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public decimal Value { get; set; }
        public User User { get; set; }
        public List<Book> Books { get; set; }
    }
}
