using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace classManager.Data.Contexts.Roles.Repositories;

public class UsersRolesRepository : TRepository<UsersRoles>, IUsersRolesRepository
{
  public UsersRolesRepository(AppDbContext context) : base(context) { }

  public async Task<bool> HasAnyRoleAsync(Guid userId, Guid tenantId, List<string> rolesNames, CancellationToken cancellationToken)
  {
    return await DbSet
    .AnyAsync(x => x.UserId == userId && x.TenantId == tenantId && rolesNames.Contains(x.Role.Name), cancellationToken);
  }

  public async Task DeleteByUserIdAndtenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where(x => x.UserId == userId && x.TenantId == tenantId));
    await SaveChangesAsync(cancellationToken);
  }

  public async Task<UsersRoles?> FindByIdWithInclude(Guid id, Guid tenantId)
  {
    return await DbSet.Include(x => x.Role).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);
  }

  public async Task<List<UsersRoles>> FindByUserId(Guid userId)
  {
    return await DbSet
    .Include(x => x.Role)
    .Include(x => x.Tenant)
    .Where(x => x.UserId == userId)
    .ToListAsync();
  }

  public async Task<List<UsersRoles>> GetStudentsRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Include(x => x.Role).Where(x => x.UserId == userId && x.TenantId == tenantId && x.Role.Name == "student").ToListAsync(cancellationToken);
  }

  public async Task<List<UsersRoles>> ListByRoleAsync(Guid tenantId, List<string> rolesNames, List<Guid> usersIds)
  {
    return await DbSet.Include(x => x.User).Where(x => x.TenantId == tenantId && (rolesNames.IsNullOrEmpty() || rolesNames.Contains(x.Role.Name) && (usersIds.IsNullOrEmpty() || usersIds.Contains(x.UserId)))).ToListAsync();
  }

  public async Task<List<UsersRoles>> ListUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where(x => x.UserId == userId && x.TenantId == tenantId).ToListAsync(cancellationToken);
  }

  public async Task<List<UsersRoles>> GetByUserIdAndRoleName(Guid userId, List<string> rolesNames)
  {
    return await DbSet
    .Where(x => x.UserId == userId && rolesNames.Contains(x.Role.Name))
    .ToListAsync();
  }
}