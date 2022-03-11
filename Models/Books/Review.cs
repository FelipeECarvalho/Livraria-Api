using Livraria.Models.Users;

namespace Livraria.Models.Books
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Stars { get; set; }
        public string Slug { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
