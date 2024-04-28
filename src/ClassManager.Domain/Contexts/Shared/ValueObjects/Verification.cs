using ClassManager.Domain.Shared.ValueObjects;

namespace ClassManager.Domain.Contexts.Shared.ValueObjects
{
  public class Verification : ValueObject
  {
    public Verification()
    {
    }

    public string Code { get; } = Guid.NewGuid().ToString("N")[..6].ToUpper();
    public DateTime? ExpiresAt { get; private set; } = DateTime.UtcNow.AddMinutes(5);
    public DateTime? VerifiedAt { get; private set; } = null;
    public bool IsActive => VerifiedAt != null && ExpiresAt == null;

    public void Verify(string code)
    {
      if (IsActive)
        AddNotification("Verification.IsActive", "This account has already been activated");

      if (ExpiresAt < DateTime.UtcNow)
        AddNotification("Verification.ExpiresAt", "This code has already expired");

      if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
        AddNotification("Verification.Code", "Invalide code");

      if (Valid)
      {
        ExpiresAt = null;
        VerifiedAt = DateTime.UtcNow;
      }
    }
  }
}