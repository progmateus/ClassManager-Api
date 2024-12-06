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

public class CreateTenantAddressHandler : Notifiable
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  private readonly IAddressRepository _addressRepository;

  public CreateTenantAddressHandler(
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateAddressCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var address = new Address(command.Street, command.City, command.State, null, tenantId);

    await _addressRepository.CreateAsync(address, new CancellationToken());

    return new CommandResult(true, "ADDRESS_CREATED", address, null, 201);
  }
}
