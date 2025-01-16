using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class UpdateSubscriptionCommand : Notifiable
{
  public ESubscriptionStatus? Status { get; set; }
  public Guid? TenantPlanId { get; set; }
  public Guid? PlanId { get; set; }
}