using ClassManager.Domain.Contexts.Plans.Commands;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class CreatePlandHandler : Notifiable, IHandler<PlanCommand>
{
  private readonly IPlanRepository _planRepository;
  private readonly IStripeService _stripeService;
  public CreatePlandHandler(
    IPlanRepository planRepository,
    IStripeService stripeService
    )
  {
    _planRepository = planRepository;
    _stripeService = stripeService;
  }
  public async Task<ICommandResult> Handle(PlanCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var plan = new Plan(command.Name, command.Description, command.StudentsLimit, command.ClassesLimit, command.Price);

    var stripeProduct = _stripeService.CreateProduct(plan.Id, "application", plan.Name, null);
    _stripeService.CreatePrice(plan.Id, null, stripeProduct.Id, plan.Price * 100);

    plan.SetStripeProductId(stripeProduct.Id);

    await _planRepository.CreateAsync(plan, new CancellationToken());

    return new CommandResult(true, "PLAN_CREATED", plan, null, 201);
  }
}
