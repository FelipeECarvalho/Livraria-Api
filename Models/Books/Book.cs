namespace Livraria.Models.Books
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Summary { get; set; }
        public int PagesNumber { get; set; }
        public string Slug { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<Sale> Sales { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
