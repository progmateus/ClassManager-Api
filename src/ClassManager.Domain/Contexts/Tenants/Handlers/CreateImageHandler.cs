using AutoMapper;
using ClassManager.Api.Contexts.Shared.Utils;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class CreateImageHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IImageRepository _imageRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public CreateImageHandler(
    ITenantRepository tenantRepository,
    IMapper mapper,
    IAccessControlService accessControlService,
    IImageRepository imageRepository
    )
  {
    _tenantRepository = tenantRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
    _imageRepository = imageRepository;
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

    var tenant = await _tenantRepository.FindAsync(x => x.Id == tenantId, [x => x.Images]);

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    if (!tenant.Images.IsNullOrEmpty() && tenant.Images.Count >= 9)
    {
      return new CommandResult(false, "ERR_IMAGES_LIMIX_EXCEEDED", null, null, 400);
    }

    var fileName = $"{tenant.Id}-{Guid.NewGuid()}.{extension}";

    var image = new Image(fileName, tenant.Id);

    try
    {
      await FileService.Upload(command.Image, fileName, "images");
    }
    catch
    {
      return new CommandResult(false, "ERR_INTERNAL_SERVER_ERROR", null, null, 500);
    }

    tenant.Images.Add(image);

    await _tenantRepository.UpdateAsync(tenant, new CancellationToken());

    return new CommandResult(true, "IMAGE_ADDED", _mapper.Map<TenantViewModel>(tenant), null, 201);
  }
}
