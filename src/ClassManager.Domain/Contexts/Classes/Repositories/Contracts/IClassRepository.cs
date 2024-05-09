using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IClassRepository : IRepository<Class>
{
  Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken);
  Task<Class> GetByIdAndTenantId(Guid tenantId, Guid planId, CancellationToken cancellationToken);
  List<Class> ListByTenantId(Guid tenantId, CancellationToken cancellationToken);
}