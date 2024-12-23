using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Contexts.Addresses.Entites;

namespace ClassManager.Domain.Contexts.Classes.Entities
{
  public class Class : TenantEntity
  {
    protected Class()
    {

    }
    public Class(string name, Guid tenantId, string description, Guid? timeTableId)
    {
      Name = name;
      TenantId = tenantId;
      Description = description;
      TimeTableId = timeTableId;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = null!;
    public Guid? TimeTableId { get; private set; }
    public Guid? AddressId { get; private set; }
    public string? Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Tenant? Tenant { get; private set; }
    public TimeTable? TimeTable { get; private set; }
    public Address? Address { get; private set; }
    public IList<User> Users { get; private set; }
    public IList<TeachersClasses> TeachersClasses { get; private set; } = [];
    public IList<StudentsClasses> StudentsClasses { get; private set; } = [];
    public IList<ClassDay> ClassDays { get; private set; } = [];

    public void ChangeClass(string name, string description)
    {
      Name = name;
      Description = description;
    }

    public void UpdateTimeTable(Guid timeTableId)
    {
      TimeTableId = timeTableId;
    }

    public void UpdateAddress(Guid? addressId)
    {
      AddressId = addressId;
    }
  }
}