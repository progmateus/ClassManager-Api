using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Invoices.Handlers;

public class CreateTenantInvoiceHandler
{
  private IPlanRepository _planRepository;
  private ITenantInvoiceRepository _tenantInvoiceRepository;
  private readonly IAccessControlService _accessControlService;

  public CreateTenantInvoiceHandler(
    IPlanRepository planRepository,
    ITenantInvoiceRepository tenantInvoiceRepository,
    IAccessControlService accessControlService
    )
  {
    _planRepository = planRepository;
    _tenantInvoiceRepository = tenantInvoiceRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var plan = await _planRepository.GetByIdAsync(tenantId, default);

    if (plan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var tenantInvoice = new TenantInvoice(tenantId, plan.Id, plan.Price);

    tenantInvoice.UpdateStatus(EInvoiceStatus.PAYED);

    await _tenantInvoiceRepository.CreateAsync(tenantInvoice, new CancellationToken());

    return new CommandResult(true, "INVOICE_UPDATED", tenantInvoice, null, 200);
  }
}
