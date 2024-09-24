namespace ClassManager.Shared.Services
{
  public class ServiceResult : IServiceResult
  {

    public ServiceResult(bool success, string message, int status = 400)
    {
      IsSuccess = success;
      Message = message;
      Status = status;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int? Status { get; set; }
  }
}