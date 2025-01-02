using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Invoices.Mappings;

public class PayoutMap : IEntityTypeConfiguration<Payout>
{
  public void Configure(EntityTypeBuilder<Payout> builder)
  {
    builder.ToTable("Payouts");

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

    builder.Property(x => x.StripePayoutId)
      .HasColumnName("VARCHAR")
      .HasMaxLength(200)
      .IsRequired();

    builder.Property(x => x.Status)
      .HasColumnName("Status")
      .HasColumnType("TINYINT")
      .IsRequired();

    builder.HasOne(e => e.Tenant)
      .WithMany(t => t.Payouts)
      .HasForeignKey("TenantId")
      .HasPrincipalKey(c => c.Id)
      .IsRequired();
  }
}