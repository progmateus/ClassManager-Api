using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ImageRepository : TRepository<Image>, IImageRepository
{
  public ImageRepository(AppDbContext context) : base(context) { }
}
