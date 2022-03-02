namespace Livraria.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string Slug { get; set; }
        public IList<Evaluation> Evaluations { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<Sale> Purchases { get; set; }
        public Adress Adress { get; set; }
    }
}
