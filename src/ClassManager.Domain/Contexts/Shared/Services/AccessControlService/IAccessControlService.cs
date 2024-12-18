using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Shared.Services.AccessControlService;

public interface IAccessControlService
{
  Task<bool> IsTenantSubscriptionActiveAsync(Guid tenantId);
  Task<bool> IsTenantActiveAndChargesEnabled(Guid tenantId);
  Task<bool> HasUserAnyRoleAsync(Guid userId, Guid tenantId, List<string> rolesNames);
  Task<bool> IsUserActiveSubscriptionAsync(Guid userId, Guid tenantId);
  Task<bool> VerifyUserPendingSubscriptionsInvoices(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancelationToken = default);
  Task<bool> CheckParameterUserIdPermission(Guid? tenantId, Guid loggedUserId, Guid? userIdParameter);
  Task<bool> HasClassRoleAsync(Guid loggedUserId, Guid tenantId, Guid classId, List<string> classRolesNames);
}