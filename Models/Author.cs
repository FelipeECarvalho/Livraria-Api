namespace Livraria.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Photo { get; set; }
        public string Slug { get; set; }
        public IList<Book> Books { get; set; }
    }
}
