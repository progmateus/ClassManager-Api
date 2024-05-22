using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Roles.Repositories.Contracts;


public interface IUsersRolesRepository : IRepository<UsersRoles>
{
  Task DeleteUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<UsersRoles>> ListUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<bool> VerifyRoleExistsAsync(Guid userId, Guid tenantId, string roleName, CancellationToken cancellationToken);

}