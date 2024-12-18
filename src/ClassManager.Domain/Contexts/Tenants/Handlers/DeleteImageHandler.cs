using AutoMapper;
using ClassManager.Api.Contexts.Shared.Utils;
using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class DeleteImageHandler : ITenantDeleteAction
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IImageRepository _imageRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public DeleteImageHandler(
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
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid imageId)
  {
    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var image = await _imageRepository.FindByIdAndTenantIdAsync(imageId, tenantId, new CancellationToken());

    if (image is null)
    {
      return new CommandResult(false, "ERR_IMAGE_NOT_FOUND", null, null, 404);
    }

    try
    {
      FileService.Delete(image.Name, "images");
    }
    catch
    {
      return new CommandResult(false, "ERR_INTERNAL_SERVER_ERROR", null, null, 500);
    }

    await _imageRepository.DeleteAsync(image.Id, tenantId, new CancellationToken());

    return new CommandResult(true, "IMAGE_DELETED", new { }, null, 204);
  }
}
