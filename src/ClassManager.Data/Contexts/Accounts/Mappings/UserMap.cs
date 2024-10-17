using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Accounts.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name)
            .Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80)
            .IsRequired(true);

        builder.OwnsOne(x => x.Name)
            .Property(x => x.LastName)
            .HasColumnName("LastName")
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
            .HasDefaultValue(EUserStatus.ACTIVE)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("Type")
            .HasColumnType("TINYINT")
            .HasDefaultValue(EUserType.NORMAL)
            .IsRequired();

        builder.OwnsOne(x => x.Email)
            .OwnsOne(x => x.Verification)
            .Property(x => x.Code)
            .HasColumnName("EmailVerificationCode");

        builder.OwnsOne(x => x.Email)
            .OwnsOne(x => x.Verification)
            .Property(x => x.ExpiresAt)
            .HasColumnName("EmailVerificationExpiresAt");

        builder.OwnsOne(x => x.Email)
            .OwnsOne(x => x.Verification)
            .Property(x => x.VerifiedAt)
            .HasColumnName("EmailVerificationVerifiedAt");

        builder.OwnsOne(x => x.Email)
            .OwnsOne(x => x.Verification)
            .Ignore(x => x.IsActive);


        builder.OwnsOne(x => x.Password)
            .Property(x => x.Hash)
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.OwnsOne(x => x.Password)
            .Property(x => x.ResetCode)
            .HasColumnName("PasswordResetCode")
            .IsRequired();

        builder.Property(x => x.Username)
            .HasColumnName("Username")
            .HasColumnType("VARCHAR")
            .HasMaxLength(29)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnName("Phone")
            .HasColumnType("VARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.StripeCustomerId)
            .HasColumnName("StripeCustomerId")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200)
            .IsRequired(false);
    }
}