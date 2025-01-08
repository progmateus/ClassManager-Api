using ClassManager.Domain.Contexts.Addresses.ViewModels;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Plans.ViewModels;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.tenants.ViewModels;

public class TenantViewModel
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Username { get; set; }
  public string? Description { get; set; }
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Document { get; set; }
  public string? Avatar { get; set; }
  public string? StripeOnboardUrl { get; set; }
  public ESubscriptionStatus? SubscriptionStatus { get; set; }
  public long? AvailableBalance { get; set; }
  public long? PendingBalance { get; set; }
  public bool StripeChargesEnabled { get; set; }
  public int Status { get; set; }
  public int Type { get; set; }
  public Guid UserId { get; set; }
  public Guid PlanId { get; set; }
  public DateTime ExpiresDate { get; set; }
  public PlanViewModel? Plan { get; set; }
  public UserViewModel? User { get; set; }
  public IList<RoleViewModel> Roles { get; set; } = [];
  public IList<UsersRolesViewModel> UsersRoles { get; set; } = [];
  public IList<TenantPlanViewModel> TenantPlans { get; set; } = [];
  public IList<SubscriptionViewModel> Subscriptions { get; set; } = [];
  public IList<ClassViewModel> Classes { get; set; } = [];
  public IList<LinkViewModel> Links { get; set; } = [];
  public IList<ImageViewModel> Images { get; set; } = [];
  public IList<StripeCustomerViewModel> StripeCustomers { get; set; } = [];
  public IList<PayoutViewModel> Payouts { get; set; } = [];
  public IList<ExternalBankAccountViewModel> ExternalsBanksAccounts { get; set; } = [];
  public IList<AddressViewModel> Addresses { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}