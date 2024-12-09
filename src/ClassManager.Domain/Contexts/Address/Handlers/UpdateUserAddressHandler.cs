using AutoMapper;
using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using ClassManager.Domain.Contexts.Addresses.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class UpdateUserAddressHandler : Notifiable
{
  private readonly IAddressRepository _addressRepository;
  private readonly IMapper _mapper;

  public UpdateUserAddressHandler(
    IAddressRepository addressRepository,
    IMapper mapper

    )
  {
    _addressRepository = addressRepository;
    _mapper = mapper;
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

    var address = new Address(command.Street, command.City, command.State, command.Number, loggedUserId, null);

    if (addressFound is not null)
    {
      await _addressRepository.DeleteAsync(addressFound.Id, new CancellationToken());
    }

    await _addressRepository.CreateAsync(address, new CancellationToken());

    return new CommandResult(true, "ADDRESS_CREATED", _mapper.Map<AddressViewModel>(address), null, 201);
  }
}
