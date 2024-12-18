
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
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
  private IInvoiceRepository _invoiceRepository;
  private IStudentsClassesRepository _studentsClassesRepository;
  private ITeacherClassesRepository _teacherClassesRepository;

  public AccesControlService(
    IUsersRolesRepository usersRolesRepository,
    ITenantRepository tenantRepository,
    ISubscriptionRepository subscriptionRepository,
    IInvoiceRepository invoiceRepository,
    IStudentsClassesRepository studentsClassesRepository,
    ITeacherClassesRepository teacherClassesRepository
      )
  {
    _usersRolesRepository = usersRolesRepository;
    _tenantRepository = tenantRepository;
    _subscriptionRepository = subscriptionRepository;
    _invoiceRepository = invoiceRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _teacherClassesRepository = teacherClassesRepository;
  }

  public async Task<bool> CheckParameterUserIdPermission(Guid? tenantId, Guid loggedUserId, Guid? userIdParameter)
  {

    if (!userIdParameter.HasValue || userIdParameter == Guid.Empty)
    {
      return false;
    }

    if (loggedUserId.Equals(userIdParameter.Value))
    {
      return true;
    }

    if (!tenantId.HasValue || tenantId == Guid.Empty)
    {
      return false;
    }

    if (!await HasUserAnyRoleAsync(loggedUserId, tenantId.Value, ["admin"]))
    {
      return false;
    }

    return true;
  }

  public async Task<bool> HasClassRoleAsync(Guid loggedUserId, Guid tenantId, Guid classId, List<string> classRolesNames)
  {
    if (await HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return true;
    }

    if (classRolesNames.Contains("student"))
    {
      if (await _studentsClassesRepository.FindByUserIdAndClassId(classId, loggedUserId) is not null)
      {
        return true;
      }
    }

    if (classRolesNames.Contains("teacher"))
    {
      if (await _teacherClassesRepository.GetByUserIdAndClassId(classId, loggedUserId) is not null)
      {
        return true;
      }
    }

    return false;
  }

  public async Task<bool> HasUserAnyRoleAsync(Guid userId, Guid tenantId, List<string> rolesNames)
  {
    return await _usersRolesRepository.HasAnyRoleAsync(userId, tenantId, rolesNames, new CancellationToken());
  }

  public async Task<bool> IsTenantActiveAndChargesEnabled(Guid tenantId)
  {
    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());
    if (tenant is null)
    {

      return false;
    }
    return tenant.Status == ETenantStatus.ACTIVE && tenant.SubscriptionStatus == ESubscriptionStatus.ACTIVE && tenant.StripeChargesEnabled == true;
  }

  public async Task<bool> IsTenantSubscriptionActiveAsync(Guid tenantId)
  {
    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());
    if (tenant is null)
    {
      return false;
    }
    return tenant.Status == ETenantStatus.ACTIVE && tenant.SubscriptionStatus == ESubscriptionStatus.ACTIVE;
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

  public async Task<bool> VerifyUserPendingSubscriptionsInvoices(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancelationToken = default)
  {
    return await _invoiceRepository.CountUserPendingInvoicesUntilDate(userId, tenantId, initialDate, finalDate, cancelationToken) > 0;
  }
}
