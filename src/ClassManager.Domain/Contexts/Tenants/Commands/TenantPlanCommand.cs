using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class TenantPlanCommand : Notifiable, ICommand
  {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TimesOfWeek { get; set; }
    public decimal Price { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "TenantPlanCommand.Name", "Name min 3 characters")
      .HasMaxLen(Name, 40, "TenantPlanCommand.Name", "Name max 40 characters")
      .HasMinLen(Description, 3, "TenantPlanCommand.Description", "Description min 3 characters")
      .HasMaxLen(Description, 200, "TenantPlanCommand.Description", "Description max 200 characters")
      .IsGreaterOrEqualsThan(Price, 0, "TenantPlanCommand.Price", "Price is greater Or equals than then 0")
    );
    }
  }
}