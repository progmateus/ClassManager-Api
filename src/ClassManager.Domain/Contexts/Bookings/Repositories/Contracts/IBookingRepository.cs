using ClasManager.Domain.Contexts.Bookings.Entities;

namespace ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;

public interface IBookingRepository : ITRepository<Booking>
{
  Task<Booking?> GetByUserIdAndClassDayId(Guid userId, Guid classDayId);
  Task<Booking?> GetWithInclude(Guid userId, Guid bookingId);
  Task<List<Booking>> ListByUserIdAndTenantId(Guid tenantId, Guid userId);
}