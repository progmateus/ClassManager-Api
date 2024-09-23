
namespace ClassManager.Domain.Shared.Services.AccessControlService;

public interface IAccessControlService
{
  Task<bool> IsTenantSubscriptionActive(Guid tenantId);
  Task<bool> HasUserPermission(Guid userId, Guid tenantId, string roleName);
  Task<bool> IsUserActiveSubscription(Guid userId, Guid tenantId);
}