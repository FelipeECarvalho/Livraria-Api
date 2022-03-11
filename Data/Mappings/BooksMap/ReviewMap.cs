using Livraria.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Data.Mappings.BooksMap
{
    public class ReviewMap : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnName("Title")
                .HasColumnType("VARCHAR")
                .HasMaxLength(120);

            builder.Property(x => x.Body)
                .IsRequired()
                .HasColumnName("Body")
                .HasColumnType("TEXT");

            builder.Property(x => x.Stars)
                .IsRequired()
                .HasColumnName("Stars")
                .HasColumnType("INT")
                .HasMaxLength(5);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.HasIndex(x => x.Slug, "IX_Reviews_Slug")
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Reviews);

            builder.HasOne(x => x.Book)
                .WithMany(x => x.Reviews);
        }
    }
}
