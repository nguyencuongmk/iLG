using iLG.Domain.Entities;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.Infrastructure.Repositories
{
    public class UserMatchRepository(ILGDbContext context) : Repository<UserMatch>(context), IUserMatchRepository
    {
    }
}