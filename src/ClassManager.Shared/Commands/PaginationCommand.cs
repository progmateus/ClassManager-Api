namespace ClassManager.Domain.Shared.Commands
{
  public class PaginationCommand : IPaginationCommand
  {
    public int Page { get; set; } = 1;
    public string Search { get; set; } = "";
    public int Limit { get; private set; } = 30;
  }
}