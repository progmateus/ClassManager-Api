namespace ClassManager.Domain.Shared.Commands
{
  public class PaginationCommand
  {
    public int Page { get; set; } = 1;
    public string Search { get; set; } = "";
    public int Limit { get; set; } = 30;
  }
}