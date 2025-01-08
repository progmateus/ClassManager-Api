using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class ExternalBankAccountMap : IEntityTypeConfiguration<ExternalBankAccount>
{
  public void Configure(EntityTypeBuilder<ExternalBankAccount> builder)
  {
    builder.ToTable("Links");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.StripeExternalBankAccountId)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(150)")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.Name)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(150)")
      .HasMaxLength(150)
      .IsRequired(true);

    builder.Property(x => x.Country)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(100)")
      .HasMaxLength(100)
      .IsRequired(true);

    builder.Property(x => x.Last4)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(10)")
      .HasMaxLength(10)
      .IsRequired(true);

    builder.Property(x => x.Currency)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(10)")
      .HasMaxLength(10)
      .IsRequired(true);

    builder.Property(x => x.Status)
      .HasColumnName("Status")
      .HasColumnType("TINYINT")
      .IsRequired(true);

    builder.Property(x => x.RoutingNumber)
      .HasColumnName("Url")
      .HasColumnType("VARCHAR(10)")
      .HasMaxLength(10)
      .IsRequired(true);
  }
}