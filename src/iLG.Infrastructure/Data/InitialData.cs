using iLG.Domain.Entities;
using iLG.Infrastructure.Helper;

namespace iLG.Infrastructure.Data
{
    public class InitialData
    {
        public static IEnumerable<Role> Roles =>
        [
            new Role
            {
                Name = "Admin",
                Code = "ADM",
                CreatedBy = "system"
            },
            new Role
            {
                Name = "User",
                Code = "USR",
                CreatedBy = "system"
            }
        ];

        public static IEnumerable<User> Users =>
        [
            new User
            {
                Email = "admin@localhost.com",
                EmailConfirmed = true,
                PasswordHash = PasswordHelper.HashPassword("admin"),
            }
        ];

        public static IEnumerable<UserRole> UserRoles =>
        [
            new UserRole
            {
                UserId = 1,
                RoleId = 1
            }
        ];
    }
}