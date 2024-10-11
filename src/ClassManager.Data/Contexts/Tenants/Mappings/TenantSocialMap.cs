using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class TenantSocialMap : IEntityTypeConfiguration<TenantSocial>
{
  public void Configure(EntityTypeBuilder<TenantSocial> builder)
  {
    builder.ToTable("TenantsSocials");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Url)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR")
      .IsRequired(true);

    builder.Property(x => x.Type)
      .HasColumnName("Type")
      .HasColumnType("TINYINT")
      .HasDefaultValue(ESocialType.WHATSAPP)
      .IsRequired(true);

    builder.HasOne(x => x.Tenant)
      .WithMany(p => p.TenantsSocials)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(true);
  }
}