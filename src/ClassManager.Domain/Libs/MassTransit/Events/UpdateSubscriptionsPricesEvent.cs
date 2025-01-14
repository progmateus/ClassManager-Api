using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed record UpdatesubscriptionsPricesEvent(Guid tenantPlanId)
{

}