namespace Livraria.Models
{
    public class Adress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Number { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Slug { get; set; }
    }
}
