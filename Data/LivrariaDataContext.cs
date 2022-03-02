using Livraria.Data.Mappings;
using Livraria.Models;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Data
{
    public class LivrariaDataContext : DbContext
    {
        public DbSet<Adress> Adresses { get; set; }   
        public DbSet<Author> Authors { get;set; }
        public DbSet<Book> Books { get;set; }
        public DbSet<Category> Categories { get;set; }
        public DbSet<Evaluation> Evaluations { get;set; }
        public DbSet<Role> Roles { get;set; }
        public DbSet<Sale> Sales { get;set; }
        public DbSet<User> Users { get;set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(Configuration.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdressMap());
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
