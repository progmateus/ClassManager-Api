using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class LinkCommand
  {
    public string Url { get; set; } = null!;
    public int Type { get; set; }
  }
}