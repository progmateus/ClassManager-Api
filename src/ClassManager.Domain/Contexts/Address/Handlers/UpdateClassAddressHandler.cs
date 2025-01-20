using AutoMapper;
using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using ClassManager.Domain.Contexts.Addresses.ViewModels;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateClassAddressHandler : Notifiable
{
  private readonly IAddressRepository _addressRepository;
  private readonly IClassRepository _classRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IMapper _mapper;

  public UpdateClassAddressHandler(
    IAddressRepository addressRepository,
    IClassRepository classRepository,
    IAccessControlService accessControlService,
    IMapper mapper

    )
  {
    _addressRepository = addressRepository;
    _classRepository = classRepository;
    _accessControlService = accessControlService;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classId, UpdateAddressCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var classEntity = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classEntity is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    if (classEntity.AddressId == command.AddressId)
    {
      return new CommandResult(false, "ERR_CHOOSE_ANOTHER_ADDRESS", null, null, 400);
    }

    var address = await _addressRepository.GetByIdAsync(command.AddressId, new CancellationToken());

    if (address is null)
    {
      return new CommandResult(false, "ERR_ADDRESS_NOT_FOUND", null, null, 404);
    }

    if (!tenantId.Equals(address.TenantId))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    classEntity.UpdateAddress(command.AddressId);

    await _classRepository.UpdateAsync(classEntity, new CancellationToken());

    return new CommandResult(true, "ADDRESS_CREATED", _mapper.Map<AddressViewModel>(address), null, 201);
  }
}
