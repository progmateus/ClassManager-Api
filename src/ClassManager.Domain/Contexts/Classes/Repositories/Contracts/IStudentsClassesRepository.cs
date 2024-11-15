using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IStudentsClassesRepository : IRepository<StudentsClasses>
{
  Task<StudentsClasses?> FindByUserIdAndClassId(Guid classId, Guid userId);
  Task<List<StudentsClasses>> ListByUserOrClassOrTenantAsync(List<Guid> usersIds, List<Guid> tenantsIds, List<Guid> classesIds, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
  Task<List<StudentsClasses>> ListByUserId(Guid userId);
  int CountByClassId(Guid classId);
  Task DeleteByUserIdAndtenantId(Guid tenantId, Guid userId, CancellationToken cancellationToken);
}