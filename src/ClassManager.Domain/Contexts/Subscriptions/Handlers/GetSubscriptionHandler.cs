using AutoMapper;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class GetSubscriptionProfileHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private IMapper _mapper;
  public GetSubscriptionProfileHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
  {
    _subscriptionRepository = subscriptionRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid subscriptionId)
  {
    var subscription = _mapper.Map<SubscriptionPreviewViewModel>(await _subscriptionRepository.GetSubscriptionProfileAsync(subscriptionId, tenantId, new CancellationToken()));

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }
    return new CommandResult(true, "SUBSCRIPTION_GOTTEN", subscription, null, 200);
  }
}