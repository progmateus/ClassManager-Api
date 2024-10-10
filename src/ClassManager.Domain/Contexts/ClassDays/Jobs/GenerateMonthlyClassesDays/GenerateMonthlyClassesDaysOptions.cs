using Quartz;

namespace ClasManager.Domain.Contexts.ClassDays.Jobs.GenerateMonthlyClassesDays;

public class GenerateMonthlyClassesDaysOptions
{
  public const string GenerateMonthlyClassesDaysOptionsKey = "GenerateMonthlyClassesDaysJob";
  public int AmountOfDays { get; set; }

}