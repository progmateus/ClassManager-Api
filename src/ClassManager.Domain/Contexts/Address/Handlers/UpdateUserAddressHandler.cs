using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateUserAddressHandler : Notifiable
{
  private readonly IAddressRepository _addressRepository;

  public UpdateUserAddressHandler(
    IAddressRepository addressRepository

    )
  {
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

    var addressFound = await _addressRepository.FindAsync(x => x.UserId == loggedUserId);

    var address = new Address(command.Street, command.City, command.State, loggedUserId, command.TenantId);

    if (addressFound is not null)
    {
      await _addressRepository.DeleteAsync(addressFound.Id, new CancellationToken());
    }

    await _addressRepository.CreateAsync(address, new CancellationToken());

    return new CommandResult(true, "ADDRESS_CREATED", address, null, 201);
  }
}
