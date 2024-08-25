using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface IStudentsClassesRepository : IRepository<StudentsClasses>
{
  Task<StudentsClasses> GetByUserIdAndClassId(Guid classId, Guid userId);
  Task<List<StudentsClasses>> GetByUserIdAndTenantId(Guid tenantId, Guid userId);
  int CountByClassId(Guid classId);
  Task DeleteByUserIdAndtenantId(Guid tenantId, Guid userId, CancellationToken cancellationToken);
}