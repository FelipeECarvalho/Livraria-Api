namespace Livraria.ViewModels.Books
{
    public class UpdateBookViewModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Summary { get; set; }
        public int PagesNumber { get; set; }
    }
}
