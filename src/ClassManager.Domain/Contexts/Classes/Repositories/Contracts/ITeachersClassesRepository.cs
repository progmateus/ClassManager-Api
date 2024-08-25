using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
public interface ITeacherClassesRepository : IRepository<TeachersClasses>
{
  Task<TeachersClasses> GetByUserIdAndClassId(Guid classId, Guid userId);
  int CountByClassId(Guid classId);
}