using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Tenants.Mappings;

public class TenantMap : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80)
            .IsRequired(true);

        builder.OwnsOne(x => x.Document)
        .Property(x => x.Number)
        .HasColumnName("Document")
        .HasColumnType("VARCHAR")
        .HasMaxLength(80)
        .IsRequired(true);

        builder.OwnsOne(x => x.Document)
        .Property(x => x.Type)
        .HasColumnName("DocumentType")
        .HasColumnType("TINYINT")
        .IsRequired(true);

        builder.Property(x => x.Avatar)
            .HasColumnName("Avatar")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.OwnsOne(x => x.Email)
            .Property(x => x.Address)
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasColumnType("TINYINT")
            .HasDefaultValue(ETenantStatus.ACTIVE)
            .IsRequired();

        builder.OwnsOne(x => x.Email)
            .Ignore(x => x.Verification);

        builder.HasOne(x => x.Plan)
            .WithMany(p => p.Tenants)
            .HasForeignKey(x => x.PlanId)
            .IsRequired(false);

        builder.Property(x => x.ExpiresDate)
            .HasColumnName("ExpiresDate");

        builder.Property(x => x.Username)
            .HasColumnName("Username")
            .HasColumnType("VARCHAR")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200)
            .IsRequired(false);


        builder.HasOne(x => x.User)
            .WithMany(u => u.Tenants)
            .HasForeignKey(x => x.UserId)
            .IsRequired(true);

        builder.Property(x => x.StripeAccountId)
            .HasColumnName("StripeAccountId")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(x => x.StripeCustomerId)
            .HasColumnName("StripeCustomerId")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(x => x.StripeSubscriptionId)
            .HasColumnName("StripeSubscriptionId")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(x => x.StripeChargesEnabled)
            .HasColumnName("StripeChargesEnabled")
            .HasDefaultValue(false)
            .IsRequired(true);

    }
}