using iLG.Domain.Entities;

namespace iLG.Infrastructure.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        bool CheckPassword(User? user, string requestPassword);

        List<string?> GetRoles(User user);
    }
}