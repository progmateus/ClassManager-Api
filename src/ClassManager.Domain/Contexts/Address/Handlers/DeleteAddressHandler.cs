using AutoMapper;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class DeleteAddressHandler : Notifiable
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  private readonly IAddressRepository _addressRepository;

  public DeleteAddressHandler(
    ITenantRepository tenantRepository,
    IMapper mapper,
    IAccessControlService accessControlService,
    IAddressRepository addressRepository

    )
  {
    _tenantRepository = tenantRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
    _addressRepository = addressRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid addressId)
  {
    var address = await _addressRepository.GetByIdAsync(addressId, new CancellationToken());

    if (address is null)
    {
      return new CommandResult(false, "ERR_ADDRESS_NOT_FOUND", null, null, 404);
    }

    if (address.UserId.HasValue && address.UserId != Guid.Empty && !loggedUserId.Equals(address.UserId.Value))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }


    if (address.TenantId.HasValue && address.TenantId != Guid.Empty && !await _accessControlService.HasUserAnyRoleAsync(loggedUserId, address.TenantId.Value, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    await _addressRepository.DeleteAsync(address.Id, new CancellationToken());

    return new CommandResult(true, "ADDRESS_DELETED", new { }, null, 201);
  }
}
