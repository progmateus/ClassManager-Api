using Quartz;

namespace ClasManager.Domain.Contexts.ClassDays.Jobs.GenerateMonthlyClassesDays;

[DisallowConcurrentExecution]
public class GenerateMonthlyClassesDaysJob : IJob
{
  public Task Execute(IJobExecutionContext context)
  {
    throw new NotImplementedException();
  }
}