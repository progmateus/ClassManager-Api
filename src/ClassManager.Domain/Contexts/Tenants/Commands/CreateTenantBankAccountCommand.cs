using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class CreateTenantBankAccountCommand : Notifiable, ICommand
  {
    public string Number { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public string AccountHolderName { get; set; } = null!;

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNullOrEmpty(Number, "Number", "Number is required")
      .IsNotNullOrEmpty(Country, "Country", "Country is required")
      .IsNotNullOrEmpty(Currency, "Currency", "Currency is required")
      .IsNotNullOrEmpty(AccountHolderName, "AccountHolderName", "AccountHolderName is required")
    );
    }
  }
}