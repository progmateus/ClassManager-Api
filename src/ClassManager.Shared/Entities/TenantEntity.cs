using System;
using Flunt.Notifications;
namespace ClassManager.Domain.Shared.Entities
{
  public abstract class TenantEntity : Notifiable, IEquatable<Guid>
  {
    protected TenantEntity()
    {
      Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public Guid TenantId { get; set; }

    public bool Equals(Guid id)
        => Id == id;

    public override int GetHashCode()
        => Id.GetHashCode();
  }
}