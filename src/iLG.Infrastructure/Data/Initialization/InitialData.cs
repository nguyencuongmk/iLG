using iLG.Domain.Entities;
using iLG.Domain.Enums;
using iLG.Infrastructure.Helpers;

namespace iLG.Infrastructure.Data.Initialization
{
    public class InitialData
    {
        public static IEnumerable<Hobby> Hobbies =>
        [
            new Hobby
            {
                Title = "Traveling",
                CreatedBy = "system"
            },
            new Hobby
            {
                Title = "Painting",
                CreatedBy = "system"
            },
            new Hobby
            {
                Title = "Film",
                CreatedBy = "system"
            },
            new Hobby
            {
                Title = "Music",
                CreatedBy = "system"
            },
            new Hobby
            {
                Title = "Sport",
                CreatedBy = "system"
            },
            new Hobby
            {
                Title = "Cuisine",
                CreatedBy = "system"
            }
        ];

        public static IEnumerable<HobbyDetail> HobbyDetails =>
        [
            new HobbyDetail
            {
                Name = "Traveling alone",
                HobbyId = 1,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Traveling with family",
                HobbyId = 1,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Lanscape painting",
                HobbyId = 2,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Portrait painting",
                HobbyId = 2,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Painting still lifes",
                HobbyId = 2,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Mavel",
                HobbyId = 3,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Korea film",
                HobbyId = 3,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Vietnam film",
                HobbyId = 3,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "TV series",
                HobbyId = 3,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Movie theaters",
                HobbyId = 3,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Horror film",
                HobbyId = 3,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "K-POP",
                HobbyId = 4,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "V-POP",
                HobbyId = 4,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "US-UK",
                HobbyId = 4,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "C-POP",
                HobbyId = 4,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Football",
                HobbyId = 5,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Basketball",
                HobbyId = 5,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Athletics",
                HobbyId = 5,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Chess",
                HobbyId = 5,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Tenis",
                HobbyId = 5,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Tokbokki",
                HobbyId = 6,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Pho",
                HobbyId = 6,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Bread",
                HobbyId = 6,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Banchan",
                HobbyId = 6,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Kimchi",
                HobbyId = 6,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Bulgogi",
                HobbyId = 6,
                CreatedBy = "system"
            },
            new HobbyDetail
            {
                Name = "Snacks",
                HobbyId = 6,
                CreatedBy = "system"
            },
        ];

        public static IEnumerable<Permission> Permissions =>
        [
            new Permission
            {
                Name = "Hobby.Add",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Hobby.View",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Hobby.Modify",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Hobby.Remove",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "HobbyDetail.Add",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "HobbyDetail.View",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "HobbyDetail.Modify",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "HobbyDetail.Remove",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Image.Add",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Image.View",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Image.Modify",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Image.Remove",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Role.Add",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Role.View",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Role.Modify",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "Role.Remove",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "User.Add",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "User.View",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "User.Modify",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "User.Remove",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "UserMatch.Add",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "UserMatch.View",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "UserMatch.Modify",
                CreatedBy = "system"
            },
            new Permission
            {
                Name = "UserMatch.Remove",
                CreatedBy = "system"
            }
        ];

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

        public static IEnumerable<RolePermission> RolePermissions
        {
            get
            {
                List<RolePermission> permissions = [];

                for (int i = 1; i <= Permissions.Count(); i++)
                {
                    RolePermission permission = new() { RoleId = 1, PermissionId = i, CreatedBy = "system" };
                    permissions.Add(permission);
                }

                return permissions;
            }
        }

        public static IEnumerable<User> Users =>
        [
            new User
            {
                Email = "admin@localhost.com",
                EmailConfirmed = true,
                PasswordHash = PasswordHasher.HashPassword("iLGCNS24!"),
                CreatedBy = "system"
            }
        ];

        public static IEnumerable<UserInfo> UserInfos =>
        [
            new UserInfo
            {
                FullName = "Administrator",
                Age = 0,
                Gender = Gender.Unknown,
                UserId = 1,
                CreatedBy = "system"
            }
        ];

        public static IEnumerable<UserInfoHobby> UserInfoHobbies =>
        [
            new UserInfoHobby
            {
                UserInfoId = 1,
                HobbyId = 1,
                CreatedBy = "system"
            }
        ];

        public static IEnumerable<UserRole> UserRoles =>
        [
            new UserRole
            {
                UserId = 1,
                RoleId = 1,
                CreatedBy = "system"
            }
        ];

        public static IEnumerable<Image> Images =>
        [
            new Image
            {
                UserInfoId = 1,
                Path = "https://png.pngtree.com/element_our/20190604/ourmid/pngtree-user-avatar-boy-image_1482937.jpg",
                Type = "Avatar",
                CreatedBy = "system"
            }
        ];
    }
}