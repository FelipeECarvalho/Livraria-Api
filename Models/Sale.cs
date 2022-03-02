using Livraria.Models.Enums;

namespace Livraria.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public decimal Value { get; set; }
        public User User { get; set; }
        public IList<Book> Books { get; set; }
    }
}
