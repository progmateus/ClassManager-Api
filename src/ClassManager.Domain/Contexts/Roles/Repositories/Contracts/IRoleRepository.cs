using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Roles.Repositories.Contracts;


public interface IRoleRepository : IRepository<Role>
{
  Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken);
  Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
}