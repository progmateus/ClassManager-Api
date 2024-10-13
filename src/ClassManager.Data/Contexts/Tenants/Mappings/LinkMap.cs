using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class LinkMap : IEntityTypeConfiguration<Link>
{
  public void Configure(EntityTypeBuilder<Link> builder)
  {
    builder.ToTable("Links");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Url)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(300)")
      .HasMaxLength(300)
      .IsRequired(true);

    builder.Property(x => x.Type)
      .HasColumnName("Type")
      .HasColumnType("TINYINT")
      .HasDefaultValue(ESocialType.WHATSAPP)
      .IsRequired(true);

    builder.HasOne(x => x.Tenant)
      .WithMany(p => p.Links)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(true);
  }
}