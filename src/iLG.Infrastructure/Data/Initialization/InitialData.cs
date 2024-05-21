using iLG.Domain.Entities;
using iLG.Domain.Enums;
using iLG.Infrastructure.Helpers;

namespace iLG.Infrastructure.Data.Initialization
{
    public class InitialData
    {
        public static IEnumerable<HobbyCategory> HobbyCategories =>
        [
            new HobbyCategory
            {
                Title = "Traveling"
            },
            new HobbyCategory
            {
                Title = "Painting"
            },
            new HobbyCategory
            {
                Title = "Film"
            },
            new HobbyCategory
            {
                Title = "Music"
            },
            new HobbyCategory
            {
                Title = "Sport"
            },
            new HobbyCategory
            {
                Title = "Cuisine"
            }
        ];

        public static IEnumerable<Hobby> Hobbies =>
        [
            new Hobby
            {
                Name = "Traveling alone",
                HobbyCategoryId = 1
            },
            new Hobby
            {
                Name = "Traveling with family",
                HobbyCategoryId = 1
            },
            new Hobby
            {
                Name = "Lanscape painting",
                HobbyCategoryId = 2
            },
            new Hobby
            {
                Name = "Portrait painting",
                HobbyCategoryId = 2
            },
            new Hobby
            {
                Name = "Painting still lifes",
                HobbyCategoryId = 2
            },
            new Hobby
            {
                Name = "Mavel",
                HobbyCategoryId = 3
            },
            new Hobby
            {
                Name = "Korea film",
                HobbyCategoryId = 3
            },
            new Hobby
            {
                Name = "Vietnam film",
                HobbyCategoryId = 3
            },
            new Hobby
            {
                Name = "TV series",
                HobbyCategoryId = 3
            },
            new Hobby
            {
                Name = "Movie theaters",
                HobbyCategoryId = 3
            },
            new Hobby
            {
                Name = "Horror film",
                HobbyCategoryId = 3
            },
            new Hobby
            {
                Name = "K-POP",
                HobbyCategoryId = 4
            },
            new Hobby
            {
                Name = "V-POP",
                HobbyCategoryId = 4
            },
            new Hobby
            {
                Name = "US-UK",
                HobbyCategoryId = 4
            },
            new Hobby
            {
                Name = "C-POP",
                HobbyCategoryId = 4
            },
            new Hobby
            {
                Name = "Football",
                HobbyCategoryId = 5
            },
            new Hobby
            {
                Name = "Basketball",
                HobbyCategoryId = 5
            },
            new Hobby
            {
                Name = "Athletics",
                HobbyCategoryId = 5
            },
            new Hobby
            {
                Name = "Chess",
                HobbyCategoryId = 5
            },
            new Hobby
            {
                Name = "Tenis",
                HobbyCategoryId = 5
            },
            new Hobby
            {
                Name = "Tokbokki",
                HobbyCategoryId = 6
            },
            new Hobby
            {
                Name = "Pho",
                HobbyCategoryId = 6
            },
            new Hobby
            {
                Name = "Bread",
                HobbyCategoryId = 6
            },
            new Hobby
            {
                Name = "Banchan",
                HobbyCategoryId = 6
            },
            new Hobby
            {
                Name = "Kimchi",
                HobbyCategoryId = 6
            },
            new Hobby
            {
                Name = "Bulgogi",
                HobbyCategoryId = 6
            },
            new Hobby
            {
                Name = "Snacks",
                HobbyCategoryId = 6
            },
        ];

        public static IEnumerable<Permission> Permissions =>
        [
            new Permission
            {
                Name = "HobbyCategory.Add"
            },
            new Permission
            {
                Name = "HobbyCategory.View"
            },
            new Permission
            {
                Name = "HobbyCategory.Modify"
            },
            new Permission
            {
                Name = "HobbyCategory.Remove"
            },
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
                    RolePermission permission = new() { RoleId = 1, PermissionId = i, };
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
                PasswordHash = PasswordHelper.HashPassword("iLGCNS24!")
            }
        ];

        public static IEnumerable<UserInfo> UserInfos =>
        [
            new UserInfo
            {
                UserId = 1,
                RelationshipId = 3,
                CompanyId = 1,
                JobId = 1,
                FullName = "Administrator",
                DateOfBirth = DateTime.MinValue,
                Gender = Gender.Unknown
            }
        ];

        public static IEnumerable<UserInfoHobby> UserInfoHobby =>
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
                Type = ImageType.Avatar
            }
        ];

        public static IEnumerable<Relationship> Relationships =>
        [
            new Relationship
            {
                Code = "LOV",
                Title = "Lover"
            },
            new Relationship
            {
                Code = "SRR",
                Title = "Serious Relationship"
            },
            new Relationship
            {
                Code = "FRS",
                Title = "Friendship"
            },
            new Relationship
            {
                Code = "DTG",
                Title = "Dating"
            },
            new Relationship
            {
                Code = "FWB",
                Title = "Friend With Benefit"
            },
            new Relationship
            {
                Code = "ONS",
                Title = "One Night Stand"
            },
            new Relationship
            {
                Code = "SGB",
                Title = "Sugar Baby"
            },
            new Relationship
            {
                Code = "SGD",
                Title = "Sugar Daddy"
            }
        ];

        public static IEnumerable<Company> Companies =>
        [
            new Company
            {
                Code = "LGCn",
                Title = "LG CNS"
            },
            new Company
            {
                Code = "LGDi",
                Title = "LG Display"
            },
            new Company
            {
                Code = "LGEl",
                Title = "LG Electronic"
            },
            new Company
            {
                Code = "LGIn",
                Title = "LG Innotek"
            },
            new Company
            {
                Code = "LGCh",
                Title = "LG Chem"
            },
            new Company
            {
                Code = "LGEs",
                Title = "LG Energy Solution"
            },
            new Company
            {
                Code = "LGHh",
                Title = "LG Household & Health Care"
            },
            new Company
            {
                Code = "LGUp",
                Title = "LG U+"
            },
            new Company
            {
                Code = "LGHv",
                Title = "LG HelloVision Corp."
            },
            new Company
            {
                Code = "LGMd",
                Title = "LG Development Institute"
            },
            new Company
            {
                Code = "LGSp",
                Title = "LG Sports"
            }
        ];

        public static IEnumerable<Job> Jobs =>
        [
            new Job
            {
                Code = "SE01",
                Title = "Full-Stack Developer"
            },
            new Job
            {
                Code = "SE02",
                Title = "Front-End Developer"
            },
            new Job
            {
                Code = "SE03",
                Title = "Back-End Developer"
            },
            new Job
            {
                Code = "SE04",
                Title = "Mobile Developer"
            },
            new Job
            {
                Code = "SE05",
                Title = "DevOps Engineer"
            },
            new Job
            {
                Code = "DA01",
                Title = "Data Scientist"
            },
            new Job
            {
                Code = "DA02",
                Title = "Data Analyst"
            },
            new Job
            {
                Code = "DA03",
                Title = "Business Analyst"
            },
            new Job
            {
                Code = "PP01",
                Title = "Project Manager"
            },
            new Job
            {
                Code = "PP02",
                Title = "Product Owner"
            },
            new Job
            {
                Code = "DS01",
                Title = "UI/UX Designer"
            },
            new Job
            {
                Code = "DS02",
                Title = "Visual Designer"
            },
            new Job
            {
                Code = "BS01",
                Title = "Product Marketing Manager"
            },
            new Job
            {
                Code = "BS02",
                Title = "Growth Marketer"
            },
            new Job
            {
                Code = "BS03",
                Title = "Business Partnerships Manager"
            },
            new Job
            {
                Code = "HR01",
                Title = "Human Resources Manager"
            },
            new Job
            {
                Code = "HR02",
                Title = "Talent Acquisition Manager"
            },
            new Job
            {
                Code = "HR03",
                Title = "Training and Development Manager"
            },
            new Job
            {
                Code = "HR04",
                Title = "Compensation and Benefits Manager"
            }
        ];
    }
}