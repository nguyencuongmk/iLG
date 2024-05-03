using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserRole : Entity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}