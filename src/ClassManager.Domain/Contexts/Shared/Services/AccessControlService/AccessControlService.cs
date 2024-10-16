
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;

namespace ClassManager.Domain.Shared.Services.AccessControlService;

public class AccesControlService : IAccessControlService
{

  private IUsersRolesRepository _usersRolesRepository;
  private ITenantRepository _tenantRepository;
  private ISubscriptionRepository _subscriptionRepository;


  public AccesControlService(
    IUsersRolesRepository usersRolesRepository,
    ITenantRepository tenantRepository,
    ISubscriptionRepository subscriptionRepository
      )
  {
    _usersRolesRepository = usersRolesRepository;
    _tenantRepository = tenantRepository;
    _subscriptionRepository = subscriptionRepository;
  }

  public async Task<List<UsersRoles>> GetUserRolesAsync(Guid userId, Guid tenantId)
  {
    return await _usersRolesRepository.ListUsersRolesByUserIdAndTenantId(userId, tenantId, new CancellationToken());
  }

  public async Task<bool> HasUserAnyRoleAsync(Guid userId, Guid tenantId, List<string> rolesNames)
  {
    return await _usersRolesRepository.HasAnyRoleAsync(userId, tenantId, rolesNames, new CancellationToken());
  }

  public async Task<bool> IsTenantSubscriptionActiveAsync(Guid tenantId)
  {
    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());
    if (tenant is null)
    {

      return false;
    }
    return tenant.Status == ETenantStatus.ACTIVE;
  }

  public async Task<bool> IsUserActiveSubscriptionAsync(Guid userId, Guid tenantId)
  {
    var subscription = await _subscriptionRepository.FindUserLatestSubscription(tenantId, userId, default);
    if (subscription is null)
    {
      return false;
    }
    return subscription.Status == ESubscriptionStatus.ACTIVE;
  }

  public Task<bool> VerifyUserPendingSubscriptionsInvoices(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancelationToken = default)
  {
    /*  return await _invoiceRepository.CountUserPendingInvoicesUntilDate(userId, tenantId, initialDate, finalDate, cancelationToken) > 0; */

    throw new NotImplementedException();
  }
}
