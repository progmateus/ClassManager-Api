using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class ImageMap : IEntityTypeConfiguration<Image>
{
  public void Configure(EntityTypeBuilder<Image> builder)
  {
    builder.ToTable("Images");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
      .HasColumnName("Name")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.HasOne(x => x.Tenant)
      .WithMany(p => p.Images)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(true);
  }
}