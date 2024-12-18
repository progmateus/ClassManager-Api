using Microsoft.AspNetCore.Http;

namespace ClassManager.Api.Contexts.Shared.Utils;

public static class FileService
{
  public async static Task Upload(IFormFile file, string fileName, string folder = "avatars")
  {
    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/files/{folder}");
    using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
    await file.CopyToAsync(stream);
  }

  public static void Delete(string fileName, string folder = "avatars")
  {
    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/files/{folder}");

    if (File.Exists(Path.Combine(path, fileName)))
    {
      File.Delete(Path.Combine(path, fileName));
    }
  }

}