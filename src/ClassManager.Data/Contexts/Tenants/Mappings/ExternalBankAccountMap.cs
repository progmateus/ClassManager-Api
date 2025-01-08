using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class ExternalBankAccountMap : IEntityTypeConfiguration<ExternalBankAccount>
{
  public void Configure(EntityTypeBuilder<ExternalBankAccount> builder)
  {
    builder.ToTable("ExternalsBanksAccounts");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.StripeExternalBankAccountId)
      .HasColumnName("StripeExternalBankAccountId")
      .HasColumnType("VARCHAR(150)")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.Name)
      .HasColumnName("Name")
      .HasColumnType("VARCHAR(150)")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.Country)
      .HasColumnName("Country")
      .HasColumnType("VARCHAR(100)")
      .HasMaxLength(100)
      .IsRequired(true);

    builder.Property(x => x.Last4)
      .HasColumnName("Last4")
      .HasColumnType("VARCHAR(10)")
      .HasMaxLength(10)
      .IsRequired(true);

    builder.Property(x => x.Currency)
      .HasColumnName("Currency")
      .HasColumnType("VARCHAR(10)")
      .HasMaxLength(10)
      .IsRequired(true);

    builder.Property(x => x.Status)
      .HasColumnName("Status")
      .HasColumnType("TINYINT")
      .IsRequired(true);

    builder.Property(x => x.RoutingNumber)
      .HasColumnName("RoutingNumber")
      .HasColumnType("VARCHAR(10)")
      .HasMaxLength(10)
      .IsRequired(true);

    builder.HasOne(x => x.Tenant)
    .WithMany(p => p.ExternalsBanksAccounts)
    .HasForeignKey(x => x.TenantId)
    .IsRequired(true);
  }
}