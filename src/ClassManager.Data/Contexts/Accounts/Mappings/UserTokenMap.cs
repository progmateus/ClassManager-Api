using ClassManager.Domain.Contexts.Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Accounts.Mappings;

public class UsersTokens : IEntityTypeConfiguration<UserToken>
{
  public void Configure(EntityTypeBuilder<UserToken> builder)
  {
    builder.ToTable("UsersTokens");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.RefreshToken)
        .HasColumnName("Name")
        .HasColumnType("VARCHAR")
        .HasMaxLength(200)
        .IsRequired();

    builder.Property(x => x.ExpiresAt)
      .HasColumnName("ExpiresAt")
      .HasColumnType("DATETIME")
      .IsRequired();

    builder.HasOne(e => e.User)
      .WithOne()
      .HasForeignKey<UserToken>(x => x.UserId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);
  }
}