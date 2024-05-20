using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IClassDayRepository : IRepository<ClassDay>
{
  Task<ClassDay> GetByTenantIdAndClassId(Guid tenantId, Guid classId);
}