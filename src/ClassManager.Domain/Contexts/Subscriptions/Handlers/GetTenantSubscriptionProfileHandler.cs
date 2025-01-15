using AutoMapper;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class GetTenantSubscriptionProfileHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public GetTenantSubscriptionProfileHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper, IAccessControlService accessControlService
)
  {
    _subscriptionRepository = subscriptionRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var latestSubscription = await _subscriptionRepository.FindLatestSubscription(tenantId, null, ETargetType.TENANT);

    if (latestSubscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 403);
    }

    var subscriptionProfile = _mapper.Map<SubscriptionViewModel>(await _subscriptionRepository.GetSubscriptionProfileAsync(latestSubscription.Id, tenantId, new CancellationToken()));

    if (subscriptionProfile is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    return new CommandResult(true, "SUBSCRIPTION_GOTTEN", subscriptionProfile, null, 200);
  }
}