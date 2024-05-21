using iLG.Domain.Abstractions;
using iLG.Domain.Enums;

namespace iLG.Domain.Entities
{
    public class UserInfo : Entity<int>
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int RelationshipId { get; set; }

        public virtual Relationship Relationship { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string? Nickname { get; set; }

        public string? PhoneNumber { get; set; }

        public int Height { get; set; }

        public Zodiac? Zodiac { get; set; }

        public string? Biography { get; set; }

        public virtual List<UserMatch> UserMatches { get; set; } = [];

        public virtual List<Image> Images { get; set; } = [];

        public virtual List<Hobby> Hobbies { get; set; } = [];
    }
}