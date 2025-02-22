namespace ClassManager.Shared.Services
{
  public interface IServiceResult
  {
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int? Status { get; set; }
  }
}