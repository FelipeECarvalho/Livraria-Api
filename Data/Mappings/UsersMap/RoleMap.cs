using Livraria.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Data.Mappings.UsersMap
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Roles");

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder
                .HasIndex(x => x.Slug, "IX_Roles_Slug")
                .IsUnique();


            builder.HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .UsingEntity<Dictionary<string, object>>
                (
                    "UserRoles",
                    user => user
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .HasConstraintName("FK_UserRole_UsersId")
                        .OnDelete(DeleteBehavior.Cascade),
                    role => role
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .HasConstraintName("FK_UserRole_RolesId")
                        .OnDelete(DeleteBehavior.Cascade));

        }
    }
}
