using Livraria.Data.Mappings;
using Livraria.Data.Mappings.BooksMap;
using Livraria.Data.Mappings.UsersMap;
using Livraria.Models;
using Livraria.Models.Books;
using Livraria.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Data
{
    public class LivrariaDataContext : DbContext
    {
        public LivrariaDataContext(DbContextOptions<LivrariaDataContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddressMap());
            modelBuilder.ApplyConfiguration(new AuthorMap());
            modelBuilder.ApplyConfiguration(new BookMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new EvaluationMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new SaleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
