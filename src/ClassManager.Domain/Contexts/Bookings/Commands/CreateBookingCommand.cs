using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClasManager.Domain.Contexts.Bookings.Commands;

public class CreateBookingCommand : Notifiable, ICommand
{
  public Guid? UserId { get; set; }
  public Guid ClassDayId { get; set; }

  public void Validate()
  {
    AddNotifications(new Contract()
      .Requires()
      .IsNotNull(UserId, "CreateBookingCommand.UserId", "UserId not null")
      .IsNotNull(ClassDayId, "CreateBookingCommand.ClassDayId", "ClassDayId not null")
    );
  }
}