using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IClassRepository : ITRepository<Class>
{
  Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken);
  Task<Class?> GetByIdAndTenantIdAsync(Guid tenantId, Guid classId, CancellationToken cancellationToken);
  Task<Class?> FindByIdWithTimeTable(Guid id, CancellationToken cancellationToken = default);
  Task<List<Class>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
  Task<List<Class>> GetByTenantsIds(List<Guid> tenantsIds, CancellationToken cancellationToken = default);
}