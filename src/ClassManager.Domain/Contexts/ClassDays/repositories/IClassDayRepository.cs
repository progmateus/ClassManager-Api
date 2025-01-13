using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IClassDayRepository : IRepository<ClassDay>
{
  object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate);
  Task<List<ClassDayViewModel>> ListByTenantOrClassAndDate(List<Guid> tenantIds, List<Guid> classesIds, DateTime date, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
  Task<ClassDay?> FindClassDayProfile(Guid tenantId, Guid classDayId);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
  Task DeleteAllAfterAndBeforeDate(List<Guid> classesIds, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken);

}