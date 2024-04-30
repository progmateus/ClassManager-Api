using ClassManager.Domain.Contexts.Roles.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Accounts.Mappings;

public class UsersRolesMap : IEntityTypeConfiguration<UsersRoles>
{
  public void Configure(EntityTypeBuilder<UsersRoles> builder)
  {
    builder.ToTable("UsersRoles");

    builder.HasKey(x => x.Id);

    builder.HasOne(ur => ur.User)
      .WithMany(u => u.UsersRoles)
      .HasForeignKey("UserId")
      .HasPrincipalKey(e => e.Id);

    builder.HasOne(ur => ur.Role)
      .WithMany(t => t.UsersRoles)
      .HasForeignKey("RoleId")
      .HasPrincipalKey(e => e.Id);

    builder.HasOne(ur => ur.Tenant)
      .WithMany(t => t.UsersRoles)
      .HasForeignKey("TenantId")
      .HasPrincipalKey(e => e.Id);
  }
}