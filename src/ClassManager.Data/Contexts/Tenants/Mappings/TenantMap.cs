using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Accounts.Mappings;

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

        builder.HasMany(t => t.Roles)
            .WithOne(r => r.Tenant)
            .HasForeignKey("TenantId")
            .HasPrincipalKey(e => e.Id);
    }
}