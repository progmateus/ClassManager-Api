using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IStudentsClassesRepository : IRepository<StudentsClasses>
{
  Task<StudentsClasses?> FindByUserIdAndClassId(Guid classId, Guid userId);
  Task<List<StudentsClasses>> ListByUserOrClassAndTenantAsync(List<Guid> usersIds, List<Guid> tenantsIds, List<Guid> classesIds, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
  Task<List<StudentsClasses>> ListByUserId(Guid userId);
  Task<List<StudentsClasses>> GetByUsersIdsAndClassesIds(Guid tenantId, List<Guid>? usersIds, List<Guid>? classesIds);
  int CountByClassId(Guid classId);
  Task<List<StudentsClasses>> GetByUsersIdsAndTenantActive(List<Guid> usersIds, CancellationToken cancellationToken = default);
  Task DeleteByUserIdAndtenantId(Guid tenantId, List<Guid> usersIds, CancellationToken cancellationToken);
  Task DeleteByClassId(Guid tenantId, Guid classId, CancellationToken cancellationToken);
  Task DeleteByUsersAndClasses(Guid tenantId, List<Guid> classesIds, List<Guid> usersIds, CancellationToken cancellationToken);
}