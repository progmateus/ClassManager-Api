
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Addresses.ViewModels;

public class AddressViewModel
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string Street { get; private set; }
  public string? Number { get; private set; }
  public string City { get; private set; }
  public string State { get; private set; }
  public string Country { get; private set; } = "BR";
  public string? ZipCode { get; private set; }
  public UserViewModel? User { get; set; }
  public TenantViewModel? Tenant { get; set; }
  public IList<ClassViewModel> Classes { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}