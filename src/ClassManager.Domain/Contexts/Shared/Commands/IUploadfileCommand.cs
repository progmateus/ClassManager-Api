using Microsoft.AspNetCore.Http;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class UploadFileCommand
  {
    public IFormFile Image { get; set; } = null!;
  }
}