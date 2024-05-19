using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserInfoHobbyDetail : Entity
    {
        public int UserInfoId { get; set; }

        public int HobbyDetailId { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual HobbyDetail HobbyDetail { get; set; }
    }
}