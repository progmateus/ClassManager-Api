using ClasManager.Domain.Contexts.Bookings.Entities;

namespace ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;

public interface IBookingRepository : IRepository<Booking>
{
  Task<Booking?> GetByUserIdAndClassDayId(Guid userId, Guid classDayId);
  Task<Booking?> GetWithInclude(Guid userId, Guid bookingId);
  Task<List<Booking>> ListByUserIdOrTenantIdOrClassDayIdWithPagination(Guid? tenantId, Guid? userId, Guid? classDayId, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
}