using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassManager.Api.Contexts.Shared.Utils;

public static class FileService
{
  public async static Task Upload(IFormFile file, string fileName, string folder = "images")
  {
    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/files/{folder}");
    using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
    await file.CopyToAsync(stream);
  }

  public static void Delete(string fileName, string folder = "images")
  {
    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/files/{folder}");

    if (File.Exists(Path.Combine(path, fileName)))
    {
      File.Delete(Path.Combine(path, fileName));
    }
  }

  public static FileStreamResult GetFile(string fileName, string folder = "images")
  {
    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/files/{folder}", fileName);
    FileStream stream = new FileStream(path, FileMode.Open);
    FileStreamResult result = new FileStreamResult(stream, "image/png");
    /* result.FileDownloadName = fileName; --- se quiser baixar */
    return result;
  }

}