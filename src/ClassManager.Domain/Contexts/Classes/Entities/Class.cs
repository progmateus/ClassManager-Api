using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.ClassDays.Entities;

namespace ClassManager.Domain.Contexts.Classes.Entities
{
  public class Class : Entity
  {
    protected Class()
    {

    }
    public Class(string name, Guid tenantId, string businessHour, string description)
    {
      Name = name;
      TenantId = tenantId;
      BusinessHour = businessHour;
      Description = description;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = null!;
    public Guid TenantId { get; private set; }
    public string BusinessHour { get; private set; } = null!;
    public string? Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Tenant Tenant { get; }
    public List<User> Users { get; }
    public List<TeachersClasses> TeachersClasses { get; } = [];
    public List<StudentsClasses> StudentsClasses { get; } = [];
    public List<ClassDay> ClassDays { get; } = [];

    public void ChangeClass(string name, string businessHour, string description)
    {
      Name = name;
      BusinessHour = businessHour;
      Description = description;
    }
  }
}