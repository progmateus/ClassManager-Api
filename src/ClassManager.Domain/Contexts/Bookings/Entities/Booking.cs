using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClasManager.Domain.Contexts.Bookings.Entities;

public class Booking : Entity
{
  public Booking(Guid userId, Guid classDayId)
  {
    UserId = userId;
    ClassDayId = classDayId;
  }

  public Guid UserId { get; private set; }
  public Guid ClassDayId { get; private set; }
  public IList<User> User { get; private set; } = [];
  public IList<ClassDay> ClassDay { get; private set; } = [];
}