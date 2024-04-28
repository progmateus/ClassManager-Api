using ClassManager.Domain.Contexts.Roles.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Accounts.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    builder.ToTable("Roles");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasColumnType("VARCHAR")
        .HasMaxLength(80)
        .IsRequired(true);

    builder.Property(x => x.Description)
        .HasColumnName("Description")
        .HasColumnType("VARCHAR")
        .HasMaxLength(200)
        .IsRequired(false);
  }
}