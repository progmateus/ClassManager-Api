using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;

namespace ClassManager.Domain.Contexts.Classes.Entities
{
  public class StudentsClassesViewModel
  {
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ClassId { get; private set; }
    public Class Class { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
  }
}