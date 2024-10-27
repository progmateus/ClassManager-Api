using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class StripeCustomerMap : IEntityTypeConfiguration<StripeCustomer>
{
  public void Configure(EntityTypeBuilder<StripeCustomer> builder)
  {
    builder.ToTable("StripeCustomers");

    builder.HasKey(x => x.Id);

    builder.HasOne(e => e.User)
      .WithMany(u => u.StripeCustomers)
      .HasForeignKey("UserId")
      .HasPrincipalKey(u => u.Id);

    builder.HasOne(e => e.Tenant)
      .WithMany(t => t.StripeCustomers)
      .HasForeignKey("TenantId")
      .HasPrincipalKey(t => t.Id);

    builder.Property(x => x.StripeCustomerId)
      .HasColumnName("StripeCustomerId")
      .HasColumnType("VARCHAR")
      .HasMaxLength(200)
      .IsRequired();

    builder.Property(e => e.Type)
      .HasColumnType("TINYINT")
      .IsRequired(true)
      .HasDefaultValue(EStripeCustomerType.USER);
  }
}