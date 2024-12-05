using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Addresses.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Addresses.Mappings;

public class AddressMap : IEntityTypeConfiguration<Address>
{
  public void Configure(EntityTypeBuilder<Address> builder)
  {
    builder.ToTable("Addresses");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.City)
      .HasColumnName("City")
      .HasColumnType("VARCHAR")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.State)
      .HasColumnName("State")
      .HasColumnType("VARCHAR")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.Street)
      .HasColumnName("Street")
      .HasColumnType("VARCHAR")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.Number)
      .HasColumnName("Number")
      .HasColumnType("VARCHAR")
      .HasMaxLength(5)
      .IsRequired(false);

    builder.Property(x => x.Country)
      .HasColumnName("Country")
      .HasColumnType("VARCHAR")
      .HasMaxLength(10)
      .IsRequired(true);

    builder.Property(x => x.ZipCode)
      .HasColumnName("ZipCode")
      .HasColumnType("VARCHAR")
      .HasMaxLength(20)
      .IsRequired(false);

    builder.HasOne(x => x.Tenant)
      .WithMany(u => u.Addresses)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.User)
      .WithOne(u => u.Address)
      .HasForeignKey<User>(x => x.AddressId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);
  }
}