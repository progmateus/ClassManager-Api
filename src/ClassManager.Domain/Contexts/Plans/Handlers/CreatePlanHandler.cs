using ClassManager.Domain.Contexts.Plans.Commands;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Services;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class CreatePlandHandler : Notifiable, IHandler<PlanCommand>
{
  private readonly IPlanRepository _planRepository;
  private readonly IPaymentService _paymentService;
  public CreatePlandHandler(
    IPlanRepository planRepository,
    IPaymentService paymentService
    )
  {
    _planRepository = planRepository;
    _paymentService = paymentService;
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

    var stripeProduct = _paymentService.CreateProduct(plan.Id, EProductOwner.APPLICATION, plan.Name, null, null);
    var stripePrice = _paymentService.CreatePrice(plan.Id, null, stripeProduct.Id, plan.Price * 100, null);

    plan.SetStripeProductId(stripeProduct.Id);
    plan.SetStripePriceId(stripePrice.Id);

    await _planRepository.CreateAsync(plan, new CancellationToken());

    return new CommandResult(true, "PLAN_CREATED", plan, null, 201);
  }
}
