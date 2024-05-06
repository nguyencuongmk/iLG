using iLG.Domain.Entities;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Helpers;
using iLG.Infrastructure.Repositories.Abstractions;

namespace iLG.Infrastructure.Repositories
{
    public class UserRepository(ILGDbContext context) : Repository<User>(context), IUserRepository
    {
        public bool CheckPassword(User user, string requestPassword)
        {
            if (user == null || string.IsNullOrEmpty(requestPassword))
                return false;

            return PasswordHasher.VerifyPassword(requestPassword, user.PasswordHash);
        }

        public List<string?> GetRoles(User user)
        {
            if (user == null || user.Roles.Count == 0)
                return [];

            return user.Roles.Select(x => x.Name).ToList();
        }
    }
}