using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Plans.Entities;

public class Plan : Entity
{

  protected Plan() { }
  public Plan(string name, string description, int studentsLimit, int classesLimit, decimal price)
  {
    Name = name;
    Description = description;
    StudentsLimit = studentsLimit;
    ClassesLimit = classesLimit;
    Price = price;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public int StudentsLimit { get; private set; }
  public int ClassesLimit { get; private set; }
  public decimal Price { get; private set; }
  public string? StripeProductId { get; private set; }

  public List<Tenant> Tenants { get; private set; } = [];
  public IList<Invoice> Invoices { get; private set; } = [];
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }


  public void ChangePlan(string name, string description, int studentsLimit, int classesLimit, decimal price)
  {
    Name = name;
    Description = description;
    StudentsLimit = studentsLimit;
    ClassesLimit = classesLimit;
    Price = price;
  }
  public void SetStripeProductId(string stripeCustomerId)
  {
    StripeProductId = stripeCustomerId;
  }
}