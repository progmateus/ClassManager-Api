using ClassManager.Domain.Contexts.Shared.Enums;
using Stripe;
using Stripe.Identity;

namespace ClassManager.Domain.Services.Stripe.Repositories.Contracts
{
  public interface IPaymentService
  {
    Product CreateProduct(Guid entityId, EProductOwner ownerType, string name, Guid? tenantId, string? connectedAccountId);
    Product UpdateProduct(string stripeProductId, string stripePriceId, string name, string description, string? connectedAccountId);
    Subscription CreateSubscription(Guid? entityId, Guid? userId, Guid tenantId, string stripePriceId, string stripeCustomerId, ETargetType type, string? connectedAccountId);
    void UpdateSubscriptionPlan(Guid tenantId, Guid subscriptionId, string stripeSubscriptionPriceItemId, string newStripePriceId, string? connectedAccountId);
    SubscriptionSchedule ScheduleUpdateSubscriptionPlan(string stripeSubscriptionId, string newStripePriceId, string? connectedAccountId);
    Customer CreateCustomer(string name, string email, string? connectedAccountId);
    Price CreatePrice(Guid productEntityId, Guid? tenantId, string stripeProductId, decimal priceInCents, string? connectedAccountId);
    Invoice CreateInvoice(Guid? entityId, Guid? userId, Guid tenantId, string stripeCustomerId, string stripeSubscriptionId, string? connectedAccountId);
    Invoice PayInvoice(string stripeInvoiceId, string? connectedAccountId);
    Account CreateAccount(string email);
    Subscription ResumeSubscription(string stripeSubscriptionId, string? connectedAccountId);
    Subscription CancelSubscription(string stripeSubscriptionId, string? connectedAccountId);
    void CreateBankAccount(string number, string country, string currency, string holderName, string connectedAccountId);
    void AcceptStripeTerms(string ip, string connectedAccountId);
    void CreateWebhook();
    VerificationSession CreateVerificationSession(string email, string connectedAccountId);
    AccountLink CreateAccountLink(string connectedAccountId, string? linkType = "account_onboarding");
    Balance GetBalance(string connectedAccountId);
    Account GetAccount(string stripeAccountId);
  }
}