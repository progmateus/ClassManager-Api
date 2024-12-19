using ClassManager.Api.Contexts.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Shared.Controllers;

[Route("images")]
public class ImageController : MainController
{
  [HttpGet("{imageName}")]
  public FileStreamResult GetImage(
    [FromRoute] string imageName
  )
  {
    return FileService.GetFile(imageName);
  }
}