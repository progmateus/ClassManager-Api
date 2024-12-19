using System.Text.RegularExpressions;
using AutoMapper;
using ClassManager.Api.Contexts.Shared.Utils;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class UploadTenantAvatarHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public UploadTenantAvatarHandler(
    ITenantRepository tenantRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _tenantRepository = tenantRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UploadFileCommand command)
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

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var fileName = $"{tenant.Id}-{Guid.NewGuid()}{extension}";

    if (string.IsNullOrEmpty(tenant.Avatar))
    {
      FileService.Delete(tenant.Avatar, "images");
    }

    try
    {
      await FileService.Upload(command.Image, fileName, "images");
    }
    catch
    {
      return new CommandResult(false, "ERR_INTERNAL_SERVER_ERROR", null, null, 500);
    }

    tenant.SetAvatar(fileName);

    await _tenantRepository.UpdateAsync(tenant, new CancellationToken());

    return new CommandResult(true, "AVATAR_UPLOADED", _mapper.Map<TenantViewModel>(tenant), null, 201);
  }
}
