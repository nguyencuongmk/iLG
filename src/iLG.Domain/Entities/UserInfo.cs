using iLG.Domain.Abstractions;
using iLG.Domain.Enums;

namespace iLG.Domain.Entities
{
    public class UserInfo : Entity<int>
    {
        public string FullName { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string? Nickname { get; set; }

        public string? PhoneNumber { get; set; }

        public string? RelationshipStatus { get; set; }

        public Zodiac? Zodiac { get; set; }

        public string? Biography { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual List<UserMatch> UserMatches { get; set; } = [];

        public virtual List<Image> Images { get; set; } = [];

        public virtual List<HobbyDetail> HobbyDetails { get; set; } = [];
    }
}