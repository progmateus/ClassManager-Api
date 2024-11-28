using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class CreateSubscriptionCommand : Notifiable, ICommand
{
  public Guid? UserId { get; set; }
  public Guid TenantPlanId { get; set; }
  public Guid ClassId { get; set; }
  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .IsNotNull(TenantPlanId, "TenantPlan", "TenantPlan cannot be null")
    .IsNotNull(ClassId, "ClassId", "ClassId cannot be null")
    );
  }
}