using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface ITimeTableRepository : ITRepository<TimeTable>
{
  Task<TimeTable?> FindByIdAndTenantIdWithGroupBy(Guid tenantId, Guid timeTableId);
}