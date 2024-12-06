using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class UpdateAddressCommand : Notifiable, ICommand
  {
    public Guid? TenantId { get; set; }
    public Guid AddressId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(AddressId, "AddressId", "AddressId cannot be null")
    );
    }
  }
}