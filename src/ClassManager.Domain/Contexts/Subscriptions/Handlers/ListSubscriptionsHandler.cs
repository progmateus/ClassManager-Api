using AutoMapper;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class ListSubscriptionsHandler : ITenantPaginationHandler<PaginationCommand>

{
  private ITenantRepository _tenantRepository;
  private ISubscriptionRepository _subscriptionRepository;
  private IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public ListSubscriptionsHandler(ISubscriptionRepository subscriptionRepository,
    ITenantRepository tenantRepository,
    IMapper mapper,
    IAccessControlService accessControlService

  )
  {
    _tenantRepository = tenantRepository;
    _subscriptionRepository = subscriptionRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, PaginationCommand command)
  {
    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenantExists = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());

    if (!tenantExists)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var subscriptions = _mapper.Map<List<SubscriptionViewModel>>(await _subscriptionRepository.ListSubscriptions([], [tenantId], ETargetType.USER, command.Search, skip, command.Limit, new CancellationToken()));

    return new CommandResult(false, "SUBSCRIPTIONS_LISTED", subscriptions, null, 200);
  }
}