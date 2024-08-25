using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class SubscriptionMap : IEntityTypeConfiguration<Subscription>
{
  public void Configure(EntityTypeBuilder<Subscription> builder)
  {
    builder.ToTable("Subscriptions");

    builder.HasKey(x => x.Id);

    builder.HasOne(e => e.User)
      .WithMany(u => u.Subscriptions)
      .HasForeignKey("UserId")
      .HasPrincipalKey(u => u.Id);

    builder.HasOne(e => e.TenantPlan)
      .WithMany(t => t.Subscriptions)
      .HasForeignKey("TenantPlanId")
      .HasPrincipalKey(c => c.Id);

    builder.Property(e => e.Status)
      .HasColumnType("TINYINT")
      .IsRequired(true)
      .HasDefaultValue(ESubscriptionStatus.INACTIVE);

    builder.HasOne(e => e.Tenant)
    .WithMany(t => t.Subscriptions)
    .HasForeignKey("TenantId")
    .HasPrincipalKey(t => t.Id);
  }
}