using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IClassDayRepository : IRepository<ClassDay>
{
  Task<ClassDay> GetByIdAndTenantIdAsync(Guid tenantId, Guid classDayId);
  object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate);

}