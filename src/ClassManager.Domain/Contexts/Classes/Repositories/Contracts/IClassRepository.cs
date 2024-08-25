using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IClassRepository : IRepository<Class>
{
  Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken);
  Task<Class?> GetByIdAndTenantIdAsync(Guid tenantId, Guid classId, CancellationToken cancellationToken);
  Task<List<Class>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
}