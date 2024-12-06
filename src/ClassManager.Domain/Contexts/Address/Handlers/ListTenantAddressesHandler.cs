using AutoMapper;
using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class ListTenantAddressesHandler
{
  private readonly IAccessControlService _accessControlService;
  private readonly IAddressRepository _addressRepository;

  public ListTenantAddressesHandler(
    IAccessControlService accessControlService,
    IAddressRepository addressRepository

    )
  {
    _accessControlService = accessControlService;
    _addressRepository = addressRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {
    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var addresses = await _addressRepository.ListByTenantIdAsync(tenantId);

    return new CommandResult(false, "ADDRESSES_LISTED", addresses, null, 403);
  }
}
