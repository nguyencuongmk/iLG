using iLG.Domain.Entities;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.Infrastructure.Repositories
{
    public class HobbyRepository(ILGDbContext context) : Repository<Hobby>(context), IHobbyRepository
    {
    }
}