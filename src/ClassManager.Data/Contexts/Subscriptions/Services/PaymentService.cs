using ClassManager.Domain;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using Stripe;
using Stripe.Identity;

namespace ClassManager.Data.Contexts.Tenants.Services;

public class PaymentService : IPaymentService
{

  public PaymentService()
  {
    StripeConfiguration.ApiKey = Configuration.Stripe.ApiKey;
  }

  public void AcceptStripeTerms(string ip, string connectedAccountId)
  {
    var options = new AccountUpdateOptions
    {
      TosAcceptance = new AccountTosAcceptanceOptions
      {
        Date = DateTimeOffset.FromUnixTimeSeconds(1609798905).UtcDateTime,
        Ip = ip,
      },
    };
    var service = new AccountService();
    service.Update(connectedAccountId, options);
  }

  public Subscription CancelSubscription(string stripeSubscriptionId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var service = new SubscriptionService();
    return service.Cancel(stripeSubscriptionId, null, requestOptions);
  }

  public Account CreateAccount(string email)
  {
    var options = new AccountCreateOptions
    {
      Country = "BR",
      Email = email,
      Controller = new AccountControllerOptions
      {
        Fees = new AccountControllerFeesOptions { Payer = "application" },
        Losses = new AccountControllerLossesOptions { Payments = "application" },
        StripeDashboard = new AccountControllerStripeDashboardOptions
        {
          Type = "none",
        },
        RequirementCollection = "application"
      },
      Capabilities = new AccountCapabilitiesOptions
      {
        Transfers = new AccountCapabilitiesTransfersOptions
        {
          Requested = true,
        },
        CardPayments = new AccountCapabilitiesCardPaymentsOptions
        {
          Requested = true
        },
        BoletoPayments = new AccountCapabilitiesBoletoPaymentsOptions
        {
          Requested = true
        }
      }
    };
    var service = new AccountService();
    return service.Create(options);
  }

  public void CreateBankAccount(string number, string country, string currency, string holderName, string connectedAccountId)
  {
    var options = new AccountExternalAccountCreateOptions
    {
      ExternalAccount = new AccountExternalAccountBankAccountOptions()
      {
        AccountNumber = number,
        Country = country,
        Currency = currency,
        Object = "bank_account",
        AccountHolderName = holderName,
        RoutingNumber = "110-0000",
      },
    };
    var service = new AccountExternalAccountService();
    service.Create(connectedAccountId, options);
  }

  public Customer CreateCustomer(string name, string email, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };
    var options = new CustomerCreateOptions
    {
      Name = name,
      Email = email,
    };

    var service = new CustomerService();
    return service.Create(options, requestOptions);
  }

  public Invoice CreateInvoice(Guid? entityId, Guid? userId, Guid tenantId, string stripeCustomerId, string stripeSubscriptionId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };
    var options = new InvoiceCreateOptions
    {
      Customer = stripeCustomerId,
      Metadata = new Dictionary<string, string>
      {
        { "entityId", entityId.ToString() ?? ""},
        { "userId", userId.ToString() ?? "" },
        { "tenantId", tenantId.ToString() },
      },
      PaymentSettings = new InvoicePaymentSettingsOptions
      {
        PaymentMethodTypes = ["card", "boleto"]
      }
    };
    var service = new InvoiceService();
    return service.Create(options, requestOptions);
  }

  public Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new PriceCreateOptions
    {
      Currency = "brl",
      UnitAmount = Convert.ToInt64(priceInCents),
      Recurring = new PriceRecurringOptions { Interval = "month" },
      Product = stripeProductId,
      Metadata = new Dictionary<string, string>
      {
        { "entityId", productEntityId.ToString() },
        { "tenantId", tenantId.ToString() ?? "" }

      }
    };
    var service = new PriceService();
    return service.Create(options, requestOptions);
  }

  public Product CreateProduct(Guid entityId, EProductOwner ownerType, string name, Guid? tenantId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new ProductCreateOptions
    {
      Name = name,
      Metadata = new Dictionary<string, string>
      {
        { "entityId", entityId.ToString() },
        { "tenantId", tenantId.ToString() ?? "" },
        { "ownerType", ((int)ownerType).ToString() },
      }
    };
    var service = new ProductService();
    return service.Create(options, requestOptions);
  }

  public VerificationSession CreateVerificationSession(string email, string connectedAccountId)
  {
    var options = new VerificationSessionCreateOptions
    {
      Type = "document",
      ProvidedDetails = new VerificationSessionProvidedDetailsOptions
      {
        Email = email,
      }
    };

    var service = new VerificationSessionService();
    return service.Create(options);
  }

  public Subscription CreateSubscription(Guid? entityId, Guid? userId, Guid tenantId, string stripePriceId, string stripeCustomerId, ETargetType type, string? connectedAccountId)
  {

    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new SubscriptionCreateOptions
    {
      Customer = stripeCustomerId,
      Currency = "brl",
      OffSession = true,
      PaymentBehavior = "default_incomplete",
      Items = new List<SubscriptionItemOptions>
      {
          new SubscriptionItemOptions { Price = stripePriceId },
      },
      Metadata = new Dictionary<string, string>
      {
        { "entityId", entityId.ToString() ?? "" },
        { "userId", userId.ToString() ?? "" },
        { "tenantId", tenantId.ToString() },
        { "type", ((int)type).ToString() }
      }
    };
    var service = new SubscriptionService();
    return service.Create(options, requestOptions);
  }

  public void CreateWebhook()
  {
    var options = new WebhookEndpointCreateOptions
    {
      EnabledEvents = new List<string> {
        "charge.succeeded",
        "charge.failed",
        "payment_intent.succeeded",
        "payment_intent.canceled",
        "payment_intent.payment_failed",
        "invoice.created",
        "invoice.payment_succeeded",
        "invoice.payment_failed",
        "invoice.paid",
        "invoice.finalized",
        "invoice.updated",
        "invoice.voided",
        "invoice.deleted",
        "invoice.overdue",
        "identity.verification_session.verified",
        "customer.created",
        "customer.subscription.updated",
        "customer.subscription.paused",
        "customer.subscription.pending_update_applied",
        "customer.subscription.pending_update_expired",
        "customer.subscription.resumed",
        "customer.subscription.deleted",
        "customer.subscription.updated",
        "account.updated"
      },
      Url = $"{Configuration.BaseUrl}/webhooks/stripe/listen",
      Connect = true
    };
    var service = new WebhookEndpointService();
    service.Create(options);
  }

  public Subscription ResumeSubscription(string stripeSubscriptionId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new SubscriptionResumeOptions
    {
      BillingCycleAnchor = SubscriptionBillingCycleAnchor.Now,
    };
    var service = new SubscriptionService();
    return service.Resume(stripeSubscriptionId, options, requestOptions);
  }

  public AccountLink CreateAccountLink(string connectedAccountId, string? linkType = "account_onboarding")
  {
    var options = new AccountLinkCreateOptions
    {
      Account = connectedAccountId,
      RefreshUrl = "https://example.com/refresh",
      ReturnUrl = "https://example.com/return",
      Type = linkType,
      CollectionOptions = new AccountLinkCollectionOptionsOptions
      {
        Fields = "eventually_due",
      },
    };
    var service = new AccountLinkService();
    return service.Create(options);
  }

  public Account GetAccount(string stripeAccountId)
  {

    var service = new AccountService();
    return service.Get(stripeAccountId);
  }

  public void UpdateSubscriptionPlan(Guid tenantId, Guid subscriptionId, string stripeSubscriptionPriceItemId, string newStripePriceId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };
    var options = new SubscriptionItemUpdateOptions
    {
      Metadata = new Dictionary<string, string> {
        { "tenantId", tenantId.ToString() },
        { "subscriptionId", subscriptionId.ToString() }
      },
      Price = newStripePriceId,
      ProrationBehavior = "none"
    };
    var service = new SubscriptionItemService();
    service.Update(stripeSubscriptionPriceItemId, options, requestOptions);
  }

  public Invoice PayInvoice(string stripeInvoiceId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new InvoicePayOptions
    {
      PaidOutOfBand = true
    };

    var service = new InvoiceService();
    return service.Pay(stripeInvoiceId, options, requestOptions);
  }

  public Balance GetBalance(string connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new BalanceGetOptions();
    var service = new BalanceService();
    return service.Get(options, requestOptions);
  }

  public Product UpdateProduct(string stripeProductId, string stripePriceId, string name, string description, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var options = new ProductUpdateOptions
    {
      Name = name,
      Description = description,
      DefaultPrice = stripePriceId
    };
    var service = new ProductService();
    return service.Update(stripeProductId, options, requestOptions);
  }

  public SubscriptionSchedule ScheduleUpdateSubscriptionPlan(string stripeSubscriptionId, string newStripePriceId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };

    var subscriptionScheduleOptions = new SubscriptionScheduleCreateOptions
    {
      FromSubscription = stripeSubscriptionId,
    };
    var subscriptionScheduleService = new SubscriptionScheduleService();
    var schedule = subscriptionScheduleService.Create(subscriptionScheduleOptions);

    var options = new SubscriptionScheduleUpdateOptions
    {
      Phases = new List<SubscriptionSchedulePhaseOptions>
        {
          new SubscriptionSchedulePhaseOptions
            {
              Items = new List<SubscriptionSchedulePhaseItemOptions>
                {
                  new SubscriptionSchedulePhaseItemOptions
                    {
                      Price = schedule.Phases[0].Items[0].PriceId,
                      Quantity = schedule.Phases[0].Items[0].Quantity,
                    },
                },
              StartDate = schedule.Phases[0].StartDate,
              EndDate = schedule.Phases[0].EndDate,
            },
          new SubscriptionSchedulePhaseOptions
            {
              Items = new List<SubscriptionSchedulePhaseItemOptions>
              {
                new SubscriptionSchedulePhaseItemOptions
                {
                  Price = newStripePriceId,
                  Quantity = 1,
                },
              },
              Iterations = 1,
            }
        },
    };
    var service = new SubscriptionScheduleService();
    return service.Update(schedule.Id, options, requestOptions);
  }

  public void CancelSubscriptionSchedule(string stripeSubscriptionScheduleId, string? connectedAccountId)
  {
    var requestOptions = new RequestOptions
    {
      StripeAccount = connectedAccountId ?? null,
    };
    var service = new SubscriptionScheduleService();
    service.Cancel(stripeSubscriptionScheduleId, null, requestOptions);
  }
}
