using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Invoices.Mappings;

public class PaymentMap : IEntityTypeConfiguration<Payment>
{
  public void Configure(EntityTypeBuilder<Payment> builder)
  {
    builder.ToTable("Payments");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Amount)
      .HasColumnName("Amount")
      .HasColumnType("DECIMAL")
      .IsRequired();

    builder.Property(x => x.Currency)
      .HasColumnName("Currency")
      .HasColumnType("VARCHAR")
      .HasMaxLength(20)
      .IsRequired();

    builder.Property(x => x.TargetType)
      .HasColumnName("TargetType")
      .HasColumnType("TINYINT")
      .HasDefaultValue(ETargetType.USER)
      .IsRequired();

    builder.HasOne(x => x.Tenant)
      .WithMany(u => u.Payments)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.Invoice)
      .WithOne(u => u.Payment)
      .HasForeignKey<Payment>(x => x.InvoiceId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.Cascade);
  }
}