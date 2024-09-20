using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface ITimeTableRepository : ITRepository<TimeTable>
{
  Task CreateAllByTenantId(List<TimeTable> classHours, Guid tenantId);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
}