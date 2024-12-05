using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class CreateAddressCommand : Notifiable, ICommand
  {
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Number { get; set; } = null!;
    public Guid? TenantId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNullOrEmpty(Street, "Street", "Street cannot be null")
      .IsNotNullOrEmpty(City, "City", "City cannot be null")
      .IsNotNullOrEmpty(State, "State", "State cannot be null")
    );
    }
  }
}