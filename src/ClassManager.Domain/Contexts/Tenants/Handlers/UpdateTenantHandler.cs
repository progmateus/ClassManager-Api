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
  private readonly ILinkRepository _linkRepository;

  public UpdateTenantHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService,
    ILinkRepository linkRepository

    )
  {
    _repository = tenantRepository;
    _accessControlService = accessControlService;
    _linkRepository = linkRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UpdateTenantCommand command)
  {
    List<Link> links = new();
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

    var document = new Document(command.Document);
    var email = new Email(command.Email);

    tenant.Update(command.Name, email, document, command.Description);

    if (!command.Links.IsNullOrEmpty())
    {
      foreach (var linkCommand in command.Links)
      {
        var link = new Link(linkCommand.Url, (ESocialType)linkCommand.Type, tenantId);
        links.Add(link);
      }
    }

    // agrupar validações

    AddNotifications(document, email, tenant);

    if (Invalid)
    {
      return new CommandResult(false, "ERR_VALIDATION", null, Notifications);
    }

    await _linkRepository.DeleteAllByTenantIdAsync(tenantId, default);

    await _repository.UpdateAsync(tenant, default);

    await _linkRepository.CreateRangeAsync(links, default);

    return new CommandResult(true, "TENANT_UPDATED", tenant, null, 200);
  }
}
