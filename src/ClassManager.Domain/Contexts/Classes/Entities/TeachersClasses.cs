using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Classes.Entities
{
  public class TeachersClasses : Entity
  {

    protected TeachersClasses()
    {

    }
    public TeachersClasses(Guid teacherId, Guid classId)
    {
      TeacherId = teacherId;
      ClassId = classId;
      CreatedAt = DateTime.UtcNow;
    }

    public Guid TeacherId { get; private set; }
    public Guid ClassId { get; private set; }
    public Class Class { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
  }
}