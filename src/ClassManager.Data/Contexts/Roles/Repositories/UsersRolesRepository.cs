using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace classManager.Data.Contexts.Roles.Repositories;

public class UsersRolesRepository : Repository<UsersRoles>, IUsersRolesRepository
{
  public UsersRolesRepository(AppDbContext context) : base(context) { }
  public async Task DeleteByUserIdAndtenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where(x => x.UserId == userId && x.TenantId == tenantId));
    await SaveChangesAsync(cancellationToken);
  }

  public async Task<List<UsersRoles>> GetStudentsRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Include(x => x.Role).Where(x => x.UserId == userId && x.TenantId == tenantId && x.Role.Name == "student").ToListAsync(cancellationToken);
  }

  public async Task<List<UsersRoles>> ListUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where(x => x.UserId == userId && x.TenantId == tenantId).ToListAsync(cancellationToken);
  }

  public async Task<bool> VerifyRoleExistsAsync(Guid userId, Guid tenantId, string roleName, CancellationToken cancellationToken)
  {
    return await DbSet.AnyAsync(x => x.UserId == userId && x.TenantId == tenantId && x.Role.Name == roleName, cancellationToken);
  }
}