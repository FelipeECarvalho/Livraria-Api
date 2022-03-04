namespace Livraria.ViewModels
{
    public class CreateSaleViewModel
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public List<int> BooksId { get; set; }
    }
}
