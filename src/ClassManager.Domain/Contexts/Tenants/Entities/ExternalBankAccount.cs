using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{

  //tenant split account
  public class ExternalBankAccount : TenantEntity
  {

    protected ExternalBankAccount()
    {

    }
    public ExternalBankAccount(
      Guid tenantId,
      string stripeExternalBankAccountId,
      string name,
      string country,
      string currency,
      string last4,
      string routingNumber,
      EExternalBankAccountStatus status
      )
    {
      TenantId = tenantId;
      StripeExternalBankAccountId = stripeExternalBankAccountId;
      Name = name;
      Country = country;
      Currency = currency;
      Last4 = last4;
      RoutingNumber = routingNumber;
      Status = status;
    }

    public string StripeExternalBankAccountId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string Currency { get; private set; } = null!;
    public string Last4 { get; private set; } = null!;
    public string RoutingNumber { get; private set; } = null!;
    public EExternalBankAccountStatus Status { get; private set; } = EExternalBankAccountStatus.NEW;
    public Tenant? Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
  }
}