using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
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
      .HasPrincipalKey(u => u.Id)
      .IsRequired(false);

    builder.HasOne(e => e.TenantPlan)
      .WithMany(t => t.Subscriptions)
      .HasForeignKey("TenantPlanId")
      .HasPrincipalKey(c => c.Id)
      .IsRequired(false);

    builder.HasOne(e => e.Plan)
      .WithMany(t => t.Subscriptions)
      .HasForeignKey("PlanId")
      .HasPrincipalKey(c => c.Id)
      .IsRequired(false);

    builder.HasOne(e => e.NextPlan)
      .WithOne()
      .HasForeignKey<Subscription>(x => x.NextPlanId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(e => e.NextTenantPlan)
      .WithOne()
      .HasForeignKey<Subscription>(x => x.NextTenantPlanId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(e => e.Plan)
      .WithMany(t => t.Subscriptions)
      .HasForeignKey("PlanId")
      .HasPrincipalKey(c => c.Id)
      .IsRequired(false);

    builder.HasOne(e => e.LatestInvoice)
      .WithOne()
      .HasForeignKey<Subscription>(x => x.LatestInvoiceId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);

    builder.Property(e => e.Status)
      .HasColumnType("TINYINT")
      .IsRequired(true)
      .HasDefaultValue(ESubscriptionStatus.INCOMPLETE);

    builder.Property(e => e.TargetType)
      .HasColumnType("TINYINT")
      .IsRequired(true)
      .HasDefaultValue(ETargetType.USER);

    builder.HasOne(e => e.Tenant)
      .WithMany(t => t.Subscriptions)
      .HasForeignKey("TenantId")
      .HasPrincipalKey(t => t.Id);

    builder.Property(x => x.StripeSubscriptionId)
      .HasColumnName("StripeSubscriptionId")
      .HasColumnType("VARCHAR")
      .HasMaxLength(200)
      .IsRequired(false);

    builder.Property(x => x.StripeSubscriptionPriceItemId)
      .HasColumnName("StripeSubscriptionPriceItemId")
      .HasColumnType("VARCHAR")
      .HasMaxLength(200)
      .IsRequired(false);

    builder.Property(x => x.StripeScheduleSubscriptionNextPlanId)
    .HasColumnName("StripeScheduleSubscriptionNextPlanId")
    .HasColumnType("VARCHAR")
    .HasMaxLength(200)
    .IsRequired(false);

    builder.Property(x => x.CurrentPeriodStart)
      .HasColumnName("CurrentPeriodStart")
      .IsRequired();

    builder.Property(x => x.CurrentPeriodEnd)
      .HasColumnName("CurrentPeriodEnd")
      .IsRequired();

    builder.Property(x => x.CanceledAt)
      .HasColumnName("CanceledAt")
      .IsRequired(false);
  }
}