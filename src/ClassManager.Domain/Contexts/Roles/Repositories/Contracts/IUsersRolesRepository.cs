using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Roles.Repositories.Contracts;


public interface IUsersRolesRepository : ITRepository<UsersRoles>
{
  Task DeleteByUserIdAndtenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<UsersRoles>> ListUsersRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<List<UsersRoles>> GetStudentsRolesByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task<UsersRoles?> FindByIdWithInclude(Guid id, Guid tenantId);
  Task<List<UsersRoles>> FindByUserId(Guid userId);
  Task<bool> HasAnyRoleAsync(Guid userId, Guid tenantId, List<string> rolesNames, CancellationToken cancellationToken);
  Task<List<UsersRoles>> ListByRoleAsync(Guid tenantId, List<string> rolesNames, List<Guid> usersIds, string search = "", int skip = 0, int limit = int.MaxValue);
  Task<List<UsersRoles>> GetByUserIdAndRoleName(Guid userId, List<string> rolesNames);
}