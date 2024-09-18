using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IStudentsClassesRepository : IRepository<StudentsClasses>
{
  Task<StudentsClasses?> FindByUserIdAndClassId(Guid classId, Guid userId);
  Task<List<StudentsClasses>> ListByUserOrClassOrTenantAsync(List<Guid>? usersIds, List<Guid>? tenantsIds, List<Guid>? classesIds);
  int CountByClassId(Guid classId);
  Task DeleteByUserIdAndtenantId(Guid tenantId, Guid userId, CancellationToken cancellationToken);
}