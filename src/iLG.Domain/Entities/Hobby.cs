using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class Hobby : Entity<int>
    {
        public string? Title { get; set; }

        public virtual List<HobbyDetail> HobbyDetails { get; set; } = [];

        public virtual List<UserInfo> UserInfos { get; set; } = [];
    }
}