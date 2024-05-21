using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class Company : Entity<int>
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public virtual List<UserInfo> UserInfos { get; set; }
    }
}