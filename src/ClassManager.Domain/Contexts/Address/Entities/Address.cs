using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Addresses.Entites;
public class Address : Entity
{
  protected Address()
  {

  }
  public Address(string street, string city, string state, string? number, Guid? userId, Guid? tenantId)
  {
    Street = street;
    City = city;
    State = state;
    Number = number;
    UserId = userId;
    TenantId = tenantId;
  }
  public Guid? UserId { get; private set; }
  public Guid? TenantId { get; private set; }
  public string Street { get; private set; }
  public string? Number { get; private set; }
  public string City { get; private set; }
  public string State { get; private set; }
  public string Country { get; private set; } = "BR";
  public string? ZipCode { get; private set; }
  public User? User { get; private set; }
  public Tenant? Tenant { get; private set; }
  public IList<Class> Classes { get; private set; } = [];
}