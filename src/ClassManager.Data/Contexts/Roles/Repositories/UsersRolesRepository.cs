using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace classManager.Data.Contexts.Roles.Repositories;

public class UsersRolesRepository : Repository<UsersRoles>, IUsersRolesRepository
{
  public UsersRolesRepository(AppDbContext context) : base(context) { }

  public async Task DeleteUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where(x => x.UserId == userId && x.TenantId == tenantId));
    await SaveChangesAsync(cancellationToken);
  }

  public List<UsersRoles> ListUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return DbSet.Where(x => x.UserId == userId && x.TenantId == tenantId).ToList();
  }

  public async Task<bool> VerifyRoleExistsAsync(Guid userId, Guid tenantId, Guid roleId, CancellationToken cancellationToken)
  {
    return await DbSet.AnyAsync(x => x.UserId == userId && x.TenantId == tenantId && x.RoleId == roleId);
  }
}