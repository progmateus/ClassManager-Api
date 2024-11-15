

using ClassManager.Domain.Shared.Commands;

namespace ClasManager.Domain.Contexts.Bookings.Commands;

public class ListClassesDaysCommand : IPaginationCommand
{
  public DateTime Date { get; set; }
  public Guid? TenantId { get; set; }
  public int Page { get; set; } = 1;
  public string Search { get; set; } = "";
  public int Limit { get; set; } = 30;
}