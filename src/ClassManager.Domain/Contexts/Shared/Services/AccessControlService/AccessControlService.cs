
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Microsoft.IdentityModel.Tokens;

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
  public async Task<bool> HasUserPermission(Guid userId, Guid tenantId, string roleName)
  {
    return await _usersRolesRepository.VerifyRoleExistsAsync(userId, tenantId, roleName, new CancellationToken());
  }

  public async Task<bool> IsTenantSubscriptionActive(Guid tenantId)
  {
    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());
    if (tenant is null)
    {
      return false;
    }
    return tenant.Status == ETenantStatus.ACTIVE;
  }

  public async Task<bool> IsUserActiveSubscription(Guid userId, Guid tenantId)
  {
    var subscription = await _subscriptionRepository.GetLatestSubscription(tenantId, userId, default);
    if (subscription is null)
    {
      return false;
    }
    return subscription.Status == ESubscriptionStatus.ACTIVE;
  }
}
