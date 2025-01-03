using ClassManager.Domain.Contexts.Invoices.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Invoices.Mappings;

public class BalanceMap : IEntityTypeConfiguration<Balance>
{
  public void Configure(EntityTypeBuilder<Balance> builder)
  {
    builder.ToTable("Balances");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Available)
      .HasColumnName("Amount")
      .HasColumnType("DECIMAL")
      .IsRequired();

    builder.Property(x => x.Pending)
      .HasColumnName("Amount")
      .HasColumnType("DECIMAL")
      .IsRequired();

    builder.HasOne(x => x.Tenant)
      .WithOne(u => u.Balance)
      .HasForeignKey<Balance>(x => x.TenantId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}