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
                Title = "Traveling"
            },
            new Hobby
            {
                Title = "Painting"
            },
            new Hobby
            {
                Title = "Film"
            },
            new Hobby
            {
                Title = "Music"
            },
            new Hobby
            {
                Title = "Sport"
            },
            new Hobby
            {
                Title = "Cuisine"
            }
        ];

        public static IEnumerable<HobbyDetail> HobbyDetails =>
        [
            new HobbyDetail
            {
                Name = "Traveling alone",
                HobbyId = 1
            },
            new HobbyDetail
            {
                Name = "Traveling with family",
                HobbyId = 1
            },
            new HobbyDetail
            {
                Name = "Lanscape painting",
                HobbyId = 2
            },
            new HobbyDetail
            {
                Name = "Portrait painting",
                HobbyId = 2
            },
            new HobbyDetail
            {
                Name = "Painting still lifes",
                HobbyId = 2
            },
            new HobbyDetail
            {
                Name = "Mavel",
                HobbyId = 3
            },
            new HobbyDetail
            {
                Name = "Korea film",
                HobbyId = 3
            },
            new HobbyDetail
            {
                Name = "Vietnam film",
                HobbyId = 3
            },
            new HobbyDetail
            {
                Name = "TV series",
                HobbyId = 3
            },
            new HobbyDetail
            {
                Name = "Movie theaters",
                HobbyId = 3
            },
            new HobbyDetail
            {
                Name = "Horror film",
                HobbyId = 3
            },
            new HobbyDetail
            {
                Name = "K-POP",
                HobbyId = 4
            },
            new HobbyDetail
            {
                Name = "V-POP",
                HobbyId = 4
            },
            new HobbyDetail
            {
                Name = "US-UK",
                HobbyId = 4
            },
            new HobbyDetail
            {
                Name = "C-POP",
                HobbyId = 4
            },
            new HobbyDetail
            {
                Name = "Football",
                HobbyId = 5
            },
            new HobbyDetail
            {
                Name = "Basketball",
                HobbyId = 5
            },
            new HobbyDetail
            {
                Name = "Athletics",
                HobbyId = 5
            },
            new HobbyDetail
            {
                Name = "Chess",
                HobbyId = 5
            },
            new HobbyDetail
            {
                Name = "Tenis",
                HobbyId = 5
            },
            new HobbyDetail
            {
                Name = "Tokbokki",
                HobbyId = 6
            },
            new HobbyDetail
            {
                Name = "Pho",
                HobbyId = 6
            },
            new HobbyDetail
            {
                Name = "Bread",
                HobbyId = 6
            },
            new HobbyDetail
            {
                Name = "Banchan",
                HobbyId = 6
            },
            new HobbyDetail
            {
                Name = "Kimchi",
                HobbyId = 6
            },
            new HobbyDetail
            {
                Name = "Bulgogi",
                HobbyId = 6
            },
            new HobbyDetail
            {
                Name = "Snacks",
                HobbyId = 6
            },
        ];

        public static IEnumerable<Permission> Permissions =>
        [
            new Permission
            {
                Name = "Hobby.Add"
            },
            new Permission
            {
                Name = "Hobby.View"
            },
            new Permission
            {
                Name = "Hobby.Modify"
                
            },
            new Permission
            {
                Name = "Hobby.Remove"
            },
            new Permission
            {
                Name = "HobbyDetail.Add"
            },
            new Permission
            {
                Name = "HobbyDetail.View"
            },
            new Permission
            {
                Name = "HobbyDetail.Modify"
            },
            new Permission
            {
                Name = "HobbyDetail.Remove"
            },
            new Permission
            {
                Name = "Image.Add"
            },
            new Permission
            {
                Name = "Image.View"
            },
            new Permission
            {
                Name = "Image.Modify"
            },
            new Permission
            {
                Name = "Image.Remove"
            },
            new Permission
            {
                Name = "Role.Add"
            },
            new Permission
            {
                Name = "Role.View"
            },
            new Permission
            {
                Name = "Role.Modify"
            },
            new Permission
            {
                Name = "Role.Remove"
            },
            new Permission
            {
                Name = "User.Add"
            },
            new Permission
            {
                Name = "User.View"
            },
            new Permission
            {
                Name = "User.Modify"
            },
            new Permission
            {
                Name = "User.Remove"
            },
            new Permission
            {
                Name = "UserMatch.Add"
            },
            new Permission
            {
                Name = "UserMatch.View"
            },
            new Permission
            {
                Name = "UserMatch.Modify"
            },
            new Permission
            {
                Name = "UserMatch.Remove"
            }
        ];

        public static IEnumerable<Role> Roles =>
        [
            new Role
            {
                Name = "Admin",
                Code = "ADM"
            },
            new Role
            {
                Name = "User",
                Code = "USR"
            }
        ];

        public static IEnumerable<RolePermission> RolePermissions
        {
            get
            {
                List<RolePermission> permissions = [];

                for (int i = 1; i <= Permissions.Count(); i++)
                {
                    RolePermission permission = new() { RoleId = 1, PermissionId = i,  };
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
                PasswordHash = PasswordHasher.HashPassword("iLGCNS24!")
            }
        ];

        public static IEnumerable<UserInfo> UserInfos =>
        [
            new UserInfo
            {
                FullName = "Administrator",
                Age = 0,
                Gender = Gender.Unknown,
                UserId = 1
            }
        ];

        public static IEnumerable<UserInfoHobby> UserInfoHobbies =>
        [
            new UserInfoHobby
            {
                UserInfoId = 1,
                HobbyId = 1
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

        public static IEnumerable<Image> Images =>
        [
            new Image
            {
                UserInfoId = 1,
                Path = "https://png.pngtree.com/element_our/20190604/ourmid/pngtree-user-avatar-boy-image_1482937.jpg",
                Type = "Avatar"
            }
        ];
    }
}