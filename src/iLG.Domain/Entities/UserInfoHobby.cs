using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserInfoHobby : Entity
    {
        public int UserInfoId { get; set; }

        public int HobbyId { get; set; }

        public virtual Hobby Hobby { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}