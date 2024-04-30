using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Roles.Repositories.Contracts;


public interface IUsersRolesRepository : IRepository<UsersRoles>
{
  Task DeleteUsersRolesByUserIdAndTenantId(Guid userId, Guid tenanId, CancellationToken cancellationToken);
}