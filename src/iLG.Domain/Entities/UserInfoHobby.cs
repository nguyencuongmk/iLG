using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserInfoHobby : Entity
    {
        public int UserInfoId { get; set; }

        public int HobbyId { get; set; }

        public UserInfo UserInfo { get; set; }

        public Hobby Hobby { get; set; }
    }
}