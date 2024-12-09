using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface ITeacherClassesRepository : IRepository<TeachersClasses>
{
  Task<TeachersClasses> GetByUserIdAndClassId(Guid classId, Guid userId);
  Task<List<TeachersClasses>> ListByClassId(Guid classId, Guid tenantId);
  Task<List<TeachersClasses>> GetByUsersIdsAndClassesIds(Guid tenantId, List<Guid>? usersIds, List<Guid>? classesIds);
  Task<List<TeachersClasses>> ListByUserOrClassOrTenantAsync(List<Guid> usersIds, List<Guid> tenantsIds, List<Guid> classesIds, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
  Task<List<TeachersClasses>> GetByUsersIdsAndTenantActive(List<Guid> usersIds, CancellationToken cancellationToken = default);
  int CountByClassId(Guid classId);
  Task DeleteByClassId(Guid tenantId, Guid classId, CancellationToken cancellationToken);
  Task DeleteByUsersAndClasses(Guid tenantId, List<Guid> classesIds, List<Guid> usersIds, CancellationToken cancellationToken);
}