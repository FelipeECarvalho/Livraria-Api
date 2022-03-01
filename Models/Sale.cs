namespace Livraria.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public User User { get; set; }
        public IList<Book> Books { get; set; }
    }
}
