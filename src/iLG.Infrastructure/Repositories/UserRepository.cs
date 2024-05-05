using iLG.Domain.Entities;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.Infrastructure.Repositories
{
    public class UserRepository(ILGDbContext context) : Repository<User>(context), IUserRepository
    {
    }
}