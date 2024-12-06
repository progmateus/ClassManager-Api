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

public class CreateAddressHandler : Notifiable
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  private readonly IAddressRepository _addressRepository;

  public CreateAddressHandler(
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, CreateAddressCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var userId = loggedUserId;

    if (command.TenantId.HasValue && command.TenantId != Guid.Empty)
    {

      if (!await _tenantRepository.IdExistsAsync(command.TenantId.Value, new CancellationToken()))
      {
        return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
      }

      if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, command.TenantId.Value, ["admin"]))
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
      }
      userId = Guid.Empty;
    }
    else
    {
      var addressFound = await _addressRepository.FindAsync(x => x.UserId == loggedUserId);

      if (addressFound is not null)
      {
        await _addressRepository.DeleteAsync(addressFound.Id, new CancellationToken());
      }
    }

    var address = new Address(command.Street, command.City, command.State, userId, command.TenantId);

    await _addressRepository.CreateAsync(address, new CancellationToken());

    return new CommandResult(true, "ADDRESS_CREATED", address, null, 201);
  }
}
