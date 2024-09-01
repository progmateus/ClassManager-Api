using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Roles.Repositories.Contracts;


public interface IUsersRolesRepository : ITRepository<UsersRoles>
{
  Task DeleteByUserIdAndtenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<UsersRoles>> ListUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<UsersRoles>> GetStudentsRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<bool> VerifyRoleExistsAsync(Guid userId, Guid tenantId, string roleName, CancellationToken cancellationToken);
  Task<List<UsersRoles>> ListByRoleAsync(Guid tenantId, List<string> rolesNames, List<Guid> usersIds);
}