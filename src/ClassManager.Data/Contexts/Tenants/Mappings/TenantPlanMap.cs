using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class TenantPlanMap : IEntityTypeConfiguration<TenantPlan>
{
  public void Configure(EntityTypeBuilder<TenantPlan> builder)
  {
    builder.ToTable("TenantPlans");

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
      .IsRequired(true);

    builder.Property(x => x.TimesOfweek)
      .HasColumnName("TimesOfweek")
      .HasColumnType("TINYINT")
      .IsRequired(true);

    builder.Property(x => x.Price)
      .HasColumnName("Price")
      .HasColumnType("DECIMAL")
      .IsRequired(true);

    builder.HasOne(x => x.Tenant)
      .WithMany(p => p.TenantPlans)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(true);

    builder.Property(x => x.StripeProductId)
      .HasColumnName("StripeProductId")
      .HasColumnType("VARCHAR")
      .HasMaxLength(200)
      .IsRequired(false);

    builder.Property(x => x.StripePriceId)
    .HasColumnName("StripePriceId")
    .HasColumnType("VARCHAR")
    .HasMaxLength(200)
    .IsRequired(false);
  }
}