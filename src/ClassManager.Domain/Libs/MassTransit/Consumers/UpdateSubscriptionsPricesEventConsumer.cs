using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed class UpdateSubscriptionsPricesEventConsumer : IConsumer<UpdatesubscriptionsPricesEvent>
{
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly IPaymentService _paymentService;
  private readonly ILogger<UpdateSubscriptionsPricesEventConsumer> _logger;

  public UpdateSubscriptionsPricesEventConsumer(
    ISubscriptionRepository subscriptionRepository,
    IPaymentService paymentService,
    ILogger<UpdateSubscriptionsPricesEventConsumer> logger
  )
  {
    _subscriptionRepository = subscriptionRepository;
    _logger = logger;
    _paymentService = paymentService;
  }
  public async Task Consume(ConsumeContext<UpdatesubscriptionsPricesEvent> context)
  {
    try
    {
      _logger.LogInformation("Event UpdateSubscriptionsPricesEventConsumer initialized");


      var subscriptions = await _subscriptionRepository.GetByTenantPlanIdAsync(context.Message.tenantPlanId, new CancellationToken());

      foreach (var subscription in subscriptions)
      {
        Console.WriteLine("123456789");
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