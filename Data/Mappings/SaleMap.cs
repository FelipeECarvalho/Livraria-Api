using Livraria.Models;
using Livraria.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Data.Mappings
{
    public class SaleMap : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Status)
                .HasColumnType("INT")
                .IsRequired()
                .HasColumnName("Status")
                .HasMaxLength(10);

            builder.Property(x => x.Date)
                .HasColumnType("SMALLDATETIME")
                .IsRequired()
                .HasColumnName("Date");

            builder.Property(x => x.Value)
                .HasColumnType("MONEY")
                .IsRequired()
                .HasColumnName("Value");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Purchases);

            builder.HasMany(x => x.Books)
                .WithMany(x => x.Sales)
                .UsingEntity<Dictionary<string, object>>
                (
                    "BookSales",
                    book => book
                        .HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("FK_BookSales_BookId")
                        .OnDelete(DeleteBehavior.Cascade),
                    sale => sale
                        .HasOne<Sale>()
                        .WithMany()
                        .HasForeignKey("SaleId")
                        .HasConstraintName("FK_BookSales_SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}
