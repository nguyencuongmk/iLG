using iLG.Domain.Entities;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.Infrastructure.Repositories
{
    public class ImageRepository(ILGDbContext context) : Repository<Image>(context), IImageRepository
    {
    }
}