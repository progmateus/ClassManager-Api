using Microsoft.AspNetCore.Http;

namespace ClassManager.Api.Contexts.Shared.Utils;

public static class FileService
{
  public async static Task Upload(IFormFile file, string fileName, string folderPath = "wwwroot/files/avatars")
  {
    using var stream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
    await file.CopyToAsync(stream);
  }

  public static void Delete(string fileName, string folderPath = "wwwroot/files/avatars")
  {

    if (File.Exists(Path.Combine(folderPath, fileName)))
    {
      File.Delete(Path.Combine(folderPath, fileName));
    }
  }

}