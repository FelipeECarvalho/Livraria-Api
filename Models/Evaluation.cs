namespace Livraria.Models
{
    public class Evaluation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public string Slug { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
