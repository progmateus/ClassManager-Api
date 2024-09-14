using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Bookings.ViewModels;

public class BookingViewModel
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid ClassDayId { get; set; }
  public UserViewModel? User { get; set; }
  public ClassDayViewModel? ClassDay { get; set; }
}