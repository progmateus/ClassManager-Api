using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Plans.Commands
{
  public class PlanCommand : Notifiable, ICommand
  {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int StudentsLimit { get; set; }
    public int ClassesLimit { get; set; }
    public decimal Price { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "CreatePlanCommand.Name", "Name min 3 characters")
      .HasMaxLen(Name, 40, "CreatePlanCommand.Name", "Name max 40 characters")
      .HasMinLen(Description, 3, "CreatePlanCommand.Description", "Description min 3 characters")
      .HasMaxLen(Description, 300, "CreatePlanCommand.Description", "Description max 200 characters")
      .IsBetween(Price, 0, 1000, "CreatePlanCommand.Price", "Price has to be between 0 and 1000")
    );
    }
  }
}