using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IScheduleRepository : ITRepository<ScheduleDay>
{
  Task CreateAllByTenantId(List<ScheduleDay> classHours, Guid tenantId);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
}