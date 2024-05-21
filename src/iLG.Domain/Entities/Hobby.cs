using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class Hobby : Entity<int>
    {
        public string Name { get; set; }

        public int HobbyCategoryId { get; set; }

        public virtual HobbyCategory HobbyCategory { get; set; }

        public virtual List<UserInfo> UserInfos { get; set; } = [];
    }
}