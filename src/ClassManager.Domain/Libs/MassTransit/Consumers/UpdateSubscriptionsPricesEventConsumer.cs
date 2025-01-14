using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed class UpdateSubscriptionsPricesEventConsumer : IConsumer<UpdatesubscriptionsPricesEvent>
{
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly IPaymentService _paymentService;
  private readonly ILogger<UpdateSubscriptionsPricesEventConsumer> _logger;

  public UpdateSubscriptionsPricesEventConsumer(
    ISubscriptionRepository subscriptionRepository,
    ITenantPlanRepository tenantPlanRepository,
    IPaymentService paymentService,
    ILogger<UpdateSubscriptionsPricesEventConsumer> logger
  )
  {
    _subscriptionRepository = subscriptionRepository;
    _tenantPlanRepository = tenantPlanRepository;
    _logger = logger;
    _paymentService = paymentService;
  }
  public async Task Consume(ConsumeContext<UpdatesubscriptionsPricesEvent> context)
  {
    try
    {
      _logger.LogInformation("Event UpdateSubscriptionsPricesEventConsumer initialized");

      var tenantPlan = await _tenantPlanRepository.FindAsync(x => x.Id == context.Message.tenantPlanId, [x => x.Tenant]);

      if (tenantPlan is null)
      {
        _logger.LogInformation("Event UpdateSubscriptionsPricesEventConsumer tenant plan not found");
        return;
      }

      var subscriptions = await _subscriptionRepository.GetByTenantPlanIdAsync(context.Message.tenantPlanId, new CancellationToken());

      foreach (var subscription in subscriptions)
      {
        _paymentService.UpdateSubscriptionPlan(subscription.TenantId, subscription.Id, subscription.StripeSubscriptionPriceItemId, tenantPlan.StripePriceId, tenantPlan.Tenant.StripeAccountId);
      }
      _logger.LogInformation("Event UpdateSubscriptionsPricesEventConsumer finished");
    }
    catch (Exception err)
    {
      _logger.LogInformation($"Event UpdateSubscriptionsPricesEventConsumer error: {err.Message}");
      throw new Exception(err.Message);
    }
  }
}