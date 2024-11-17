namespace ClassManager.Domain.Shared.Commands
{
  public interface IPaginationCommand
  {
    public int Page { get; set; }
    public string Search { get; set; }
    public int Limit { get; }
  }
}