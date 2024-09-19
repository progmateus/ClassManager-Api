using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IClassHourRepository : ITRepository<ClassHour>
{
  Task DeleteAllByTenantId(Guid tenantId);
  Task CreateAllByTenantId(List<ClassHour> classHours, Guid tenantId);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
}