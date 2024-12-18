using System.Text.RegularExpressions;
using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class UploadUserAvatarHandler
{
  private readonly IUserRepository _userReporitory;
  private readonly IMapper _mapper;

  public UploadUserAvatarHandler(
    IUserRepository userRepository,
    IMapper mapper
    )
  {
    _userReporitory = userRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, UploadFileCommand command)
  {

    var extension = Path.GetExtension(command.Image.FileName);

    var validExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };

    if (!validExtensions.Contains(extension))
    {
      return new CommandResult(false, "ERR_INVALID EXTENSION", null, null, 400);
    }

    long size = command.Image.Length;

    if (size > (20 * 1024 * 1024))
    {
      return new CommandResult(false, "ERR_MAX_SIZE_5MB", null, null, 400);
    }

    var user = await _userReporitory.GetByIdAsync(loggedUserId, new CancellationToken());

    if (user is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var fileName = $"{user.Id}-{Guid.NewGuid()}.{extension}";

    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/avatars");

    if (user.Avatar is not null)
    {
      File.Delete(Path.Combine(path, user.Avatar));
    }

    using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
    {
      await command.Image.CopyToAsync(stream);
    }

    user.SetAvatar(fileName);

    await _userReporitory.UpdateAsync(user, new CancellationToken());

    return new CommandResult(true, "AVATAR_UPLOADED", _mapper.Map<UserViewModel>(user), null, 201);
  }
}
