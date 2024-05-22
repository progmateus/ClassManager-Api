using ClasManager.Domain.Contexts.Bookings.Entities;

namespace ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;

public interface IBookingRepository : IRepository<Booking>
{
  Task<Booking?> GetByUserIdAndClassDayId(Guid userId, Guid classDayId);
}