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

    builder.HasOne(e => e.LatestInvoice)
    .WithOne()
    .HasForeignKey<Subscription>(x => x.LatestInvoiceId)
    .OnDelete(DeleteBehavior.Restrict);

    builder.Property(e => e.Status)
      .HasColumnType("TINYINT")
      .IsRequired(true)
      .HasDefaultValue(ESubscriptionStatus.INCOMPLETE);

    builder.HasOne(e => e.Tenant)
      .WithMany(t => t.Subscriptions)
      .HasForeignKey("TenantId")
      .HasPrincipalKey(t => t.Id);

    builder.Property(x => x.StripeSubscriptionId)
      .HasColumnName("StripeSubscriptionId")
      .HasColumnType("VARCHAR")
      .HasMaxLength(200)
      .IsRequired(false);

    builder.Property(x => x.CurrentPeriodStart)
      .HasColumnName("CurrentPeriodStart")
      .IsRequired();

    builder.Property(x => x.CurrentPeriodEnd)
      .HasColumnName("CurrentPeriodEnd")
      .IsRequired();
  }
}