using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IClassDayRepository : IRepository<ClassDay>
{
  Task<ClassDay> GetByIdAndTenantIdAsync(Guid tenantId, Guid classDayId);
  object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate);
  Task<List<ClassDay>> ListByTenantOrClassAndDate(List<Guid>? tenantIds, List<Guid>? classesIds, DateTime date);
  Task<List<ClassDay>> ListByTenantId(Guid tenantId);
}