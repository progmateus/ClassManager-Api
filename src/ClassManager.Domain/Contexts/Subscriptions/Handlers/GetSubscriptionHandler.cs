using AutoMapper;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class GetSubscriptionProfileHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public GetSubscriptionProfileHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper, IAccessControlService accessControlService
)
  {
    _subscriptionRepository = subscriptionRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid subscriptionId)
  {

    var isLoggedUserAdmin = await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]);

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin", "student"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var subscription = _mapper.Map<SubscriptionViewModel>(await _subscriptionRepository.GetSubscriptionProfileAsync(subscriptionId, tenantId, new CancellationToken()));

    if (subscription is null || (!subscription.UserId.Equals(loggedUserId) && !isLoggedUserAdmin))
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }
    return new CommandResult(true, "SUBSCRIPTION_GOTTEN", subscription, null, 200);
  }
}