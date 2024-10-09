using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface ITimeTableRepository : ITRepository<TimeTable>
{
  public Task<List<TimeTable>> GetByActiveTenants();
  public Task<List<TimeTable>> FindByIdsWithInclude(List<Guid> ids, Guid tenantId);
}