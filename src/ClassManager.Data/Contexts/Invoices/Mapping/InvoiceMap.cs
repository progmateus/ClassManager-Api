using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class InvoiceMap : IEntityTypeConfiguration<Invoice>
{
  public void Configure(EntityTypeBuilder<Invoice> builder)
  {
    builder.ToTable("UsersInvoices");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Amount)
      .HasColumnName("Amount")
      .HasColumnType("DECIMAL")
      .IsRequired();

    builder.Property(x => x.Status)
      .HasColumnName("Status")
      .HasColumnType("TINYINT")
      .HasDefaultValue(EInvoiceStatus.PENDING)
      .IsRequired();

    builder.Property(x => x.Type)
    .HasColumnName("Type")
    .HasColumnType("TINYINT")
    .HasDefaultValue(EInvoiceType.USER_SUBSCRIPTION)
    .IsRequired();

    builder.Property(x => x.TargetType)
      .HasColumnName("TargetType")
      .HasColumnType("TINYINT")
      .HasDefaultValue(EInvoiceTargetType.USER)
      .IsRequired();

    builder.HasOne(e => e.User)
      .WithMany(u => u.Invoices)
      .HasForeignKey("UserId")
      .HasPrincipalKey(u => u.Id);

    builder.HasOne(e => e.TenantPlan)
      .WithMany(t => t.Invoices)
      .HasForeignKey("TenantPlanId")
      .HasPrincipalKey(c => c.Id)
      .IsRequired(false);

    builder.HasOne(e => e.Tenant)
    .WithMany(t => t.Invoices)
    .HasForeignKey("TenantId")
    .HasPrincipalKey(c => c.Id);

    builder.HasOne(e => e.Plan)
      .WithMany(t => t.Invoices)
      .HasForeignKey("PlanId")
      .HasPrincipalKey(c => c.Id)
      .IsRequired(false);

    builder.Property(x => x.ExpiresAt)
      .HasColumnName("ExpiresAt")
      .HasColumnType("DATETIME")
      .IsRequired();
  }
}