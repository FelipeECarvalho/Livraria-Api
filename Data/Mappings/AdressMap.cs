﻿using Livraria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Data.Mappings
{
    public class AdressMap : IEntityTypeConfiguration<Adress>
    {
        public void Configure(EntityTypeBuilder<Adress> builder)
        {
            builder.ToTable("Adresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Number)
                .IsRequired()
                .HasColumnName("Number")
                .HasColumnType("INT");

            builder.Property(x => x.Street)
                .IsRequired()
                .HasColumnName("Street")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.State)
                .IsRequired()
                .HasColumnName("State")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.City)
                .IsRequired()
                .HasColumnName("City")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.District)
                .IsRequired()
                .HasColumnName("District")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasColumnName("ZipCode")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(100);

            builder.HasIndex(x => x.Slug, "IX_Adresses_Slug")
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Adress)
                .HasForeignKey<Adress>(x => x.UserId) ;
        }
    }
}
