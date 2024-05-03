using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserMatch : Entity<int>
    {
        public int UserInfoId { get; set; }

        public UserInfo UserInfo { get; set; }

        public int UserMatchedId { get; set; }
    }
}