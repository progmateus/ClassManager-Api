namespace ClassManager.Domain.Contexts.Shared.Enums
{
  public enum ESubscriptionStatus
  {
    INCOMPLETE = 1,
    INCOMPLETE_EXPIRED = 2,
    TRIALING = 3,
    ACTIVE = 4,
    PAST_DUE = 5,
    CANCELED = 6,
    PAUSED = 7,
    UNPAID = 8,
  }
}