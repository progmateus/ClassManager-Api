using AutoMapper;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class GetSubscriptionhandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private IMapper _mapper;
  public GetSubscriptionhandler(ISubscriptionRepository subscriptionRepository, ITenantPlanRepository tenantPlanrepository, IMapper mapper)
  {
    _subscriptionRepository = subscriptionRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid subscriptionId)
  {
    var subscription = _mapper.Map<SubscriptionViewModel>(await _subscriptionRepository.GetSubscriptionProfileAsync(subscriptionId, tenantId, new CancellationToken()));

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }
    return new CommandResult(true, "SUBSCRIPTION_GOTTEN", subscription, null, 200);
  }
}