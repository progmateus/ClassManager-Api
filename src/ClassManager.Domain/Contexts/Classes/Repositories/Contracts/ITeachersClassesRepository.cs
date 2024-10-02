using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface ITeacherClassesRepository : IRepository<TeachersClasses>
{
  Task<TeachersClasses> GetByUserIdAndClassId(Guid classId, Guid userId);
  Task<List<TeachersClasses>> ListByClassId(Guid classId, Guid tenantId);
  Task<List<TeachersClasses>> GetByUserIdAndTenantId(Guid tenantId, Guid userId);
  Task<List<TeachersClasses>> ListByUserOrClassOrTenantAsync(List<Guid> usersIds, List<Guid> tenantsIds, List<Guid> classesIds);
  int CountByClassId(Guid classId);
}