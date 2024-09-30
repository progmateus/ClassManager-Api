using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IClassDayRepository : IRepository<ClassDay>
{
  object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate);
  Task<List<ClassDay>> ListByTenantOrClassAndDate(List<Guid> tenantIds, List<Guid> classesIds, DateTime date);
  Task<ClassDay?> FindClassDayProfile(Guid tenantId, Guid classDayId);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
}