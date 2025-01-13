using AutoMapper;
using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Bookings.ViewModels;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ClassDayRepository : Repository<ClassDay>, IClassDayRepository
{
  private readonly IMapper _mapper;
  public ClassDayRepository(
    AppDbContext context,
    IMapper mapper
    ) : base(context)
  {
    _mapper = mapper;
  }

  public object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate)
  {
    return DbSet
      .Where(x => x.ClassId == classId && x.Date >= initiDate && x.Date <= endDate)
      .GroupBy(x => x.Status)
      .Select(g => new { status = g.Key, count = g.Count() });
  }

  public async Task<List<ClassDayViewModel>> ListByTenantOrClassAndDate(List<Guid> tenantIds, List<Guid> classesIds, DateTime date, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    var zeroTime = date.Date;
    var finalTime = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

    return await DbSet
    .AsNoTracking()
    .Include((x) => x.Bookings.Take(3))
    .ThenInclude((b) => b.User)
    .Include((x) => x.Class)
    .ThenInclude((c) => c.Tenant)
    .Include(x => x.Class)
    .ThenInclude(x => x.Address)
    .Where(x => tenantIds.Contains(x.Class.TenantId) || classesIds.Contains(x.ClassId))
    .Where(x => x.Date >= zeroTime && x.Date <= finalTime)
    .OrderBy(x => x.Date)
    .Select(x => new ClassDayViewModel
    {
      Id = x.Id,
      Name = x.Name,
      Date = x.Date,
      HourStart = x.HourStart,
      HourEnd = x.HourEnd,
      Status = x.Status,
      Observation = x.Observation,
      BookingsCount = x.Bookings.Count(),
      ClassId = x.ClassId,
      CreatedAt = x.CreatedAt,
      UpdatedAt = x.UpdatedAt,
      Class = _mapper.Map<ClassViewModel>(x.Class),
      Bookings = _mapper.Map<List<BookingViewModel>>(x.Bookings)
    })
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }

  public async Task<List<ClassDay>> ListByTenantId(Guid tenantId)
  {
    return await DbSet
      .Where(x => x.Class.TenantId == tenantId)
      .ToListAsync();
  }

  public async Task<ClassDay?> FindClassDayProfile(Guid tenantId, Guid classDayId)
  {
    return await DbSet
    .AsNoTracking()
    .Include(x => x.Class)
    .ThenInclude(x => x.TeachersClasses)
    .ThenInclude(tc => tc.User)
    .Include(x => x.Class)
    .ThenInclude((C) => C.Address)
    .Include((x) => x.Bookings)
    .ThenInclude((b) => b.User)
    .FirstOrDefaultAsync((x) => x.Class.TenantId == tenantId && x.Id == classDayId);

  }

  public async Task DeleteAllAfterAndBeforeDate(List<Guid> classesIds, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where((cd) => classesIds.Contains(cd.ClassId) && cd.Date > initialDate && cd.Date < finalDate));
    await SaveChangesAsync(cancellationToken);
  }
}
