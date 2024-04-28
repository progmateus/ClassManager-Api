using System;
using Flunt.Notifications;
namespace ClassManager.Domain.Shared.Entities
{
  public abstract class Entity : Notifiable, IEquatable<Guid>
  {
    protected Entity()
    {
      Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public bool Equals(Guid id)
        => Id == id;

    public override int GetHashCode()
        => Id.GetHashCode();
  }
}