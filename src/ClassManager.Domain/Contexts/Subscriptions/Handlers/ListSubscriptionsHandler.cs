using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class ListSubscriptionsHandler : Notifiable,
  ITenantListHandler
{
  private ITenantRepository _tenantRepository;
  private ISubscriptionRepository _subscriptionRepository;
  private IMapper _mapper;
  public ListSubscriptionsHandler(ISubscriptionRepository subscriptionRepository, ITenantRepository tenantRepository, IMapper mapper)
  {
    _tenantRepository = tenantRepository;
    _subscriptionRepository = subscriptionRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid tenantId)
  {

    var tenantExists = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());

    if (!tenantExists)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var subscriptions = _mapper.Map<List<SubscriptionPreviewViewModel>>(await _subscriptionRepository.ListSubscriptions(null, tenantId));

    return new CommandResult(false, "SUBSCRIPTIONS_LISTED", subscriptions, null, 200);
  }
}