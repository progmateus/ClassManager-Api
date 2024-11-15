using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IClassDayRepository : IRepository<ClassDay>
{
  object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate);
  Task<List<ClassDay>> ListByTenantOrClassAndDate(List<Guid> tenantIds, List<Guid> classesIds, DateTime date, string search = "", int skip = 0, int limit = 30, CancellationToken cancellationToken = default);
  Task<ClassDay?> FindClassDayProfile(Guid tenantId, Guid classDayId);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
  Task DeleteAllAfterAndBeforeDate(List<Guid> classesIds, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken);

}