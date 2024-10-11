using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateTenantHandler :
  Notifiable,
  ITenantHandler<UpdateTenantCommand>
{
  private readonly ITenantRepository _repository;
  private readonly IAccessControlService _accessControlService;



  public UpdateTenantHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService

    )
  {
    _repository = tenantRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UpdateTenantCommand command)
  {
    List<TenantSocial> tenantsSocials = new();
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenant = await _repository.GetByIdAsync(tenantId, default);

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, command.Notifications, 404);
    }

    if ((tenant.Document.Number != command.Document) && await _repository.DocumentAlreadyExistsAsync(command.Document, new CancellationToken()))
    {
      AddNotification("Document", "Document already exists");
    }


    if ((tenant.Email.Address.ToLower() != command.Email.ToLower()) && await _repository.EmailAlreadyExtstsAsync(command.Email, new CancellationToken()))
    {
      AddNotification("Email", "E-mail already exists");
    }

    var document = new Document(command.Document, EDocumentType.CNPJ);
    var email = new Email(command.Email);

    tenant.Update(command.Name, email, document, command.Description);

    if (!command.TenantsSocials.IsNullOrEmpty())
    {
      foreach (var social in command.TenantsSocials)
      {
        var tenantSocial = new TenantSocial(social.Url, social.Type, tenantId);
        tenantsSocials.Add(tenantSocial);
      }
    }

    // agrupar validações

    AddNotifications(document, email, tenant);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, Notifications);
    }

    await _repository.UpdateAsync(tenant, default);

    return new CommandResult(true, "TENANT_UPDATED", tenant, null, 200);
  }
}
