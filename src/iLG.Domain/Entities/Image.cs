using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class Image : Entity<int>
    {
        public int UserInfoId { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public string? Path { get; set; }

        public string? Type { get; set; }
    }
}