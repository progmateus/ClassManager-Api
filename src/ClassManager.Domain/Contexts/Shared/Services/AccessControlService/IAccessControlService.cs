namespace ClassManager.Domain.Shared.Services.AccessControlService;

public interface IAccessControlService
{
  Task<bool> IsTenantSubscriptionActiveAsync(Guid tenantId);
  Task<bool> HasUserRoleAsync(Guid userId, Guid tenantId, string roleName);
  Task<bool> IsUserActiveSubscriptionAsync(Guid userId, Guid tenantId);
}