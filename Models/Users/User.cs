using Livraria.Models.Books;

namespace Livraria.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string Slug { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Role> Roles { get; set; } = new();
        public List<Sale> Purchases { get; set; }
        public Address Address { get; set; }
    }
}
