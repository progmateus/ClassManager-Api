namespace ClassManager.Domain.Shared.Services.AccessControlService;

public interface IAccessControlService
{
  Task<bool> IsTenantSubscriptionActiveAsync(Guid tenantId);
  Task<bool> HasUserAnyRoleAsync(Guid userId, Guid tenantId, List<string> rolesNames);
  Task<bool> IsUserActiveSubscriptionAsync(Guid userId, Guid tenantId);
}