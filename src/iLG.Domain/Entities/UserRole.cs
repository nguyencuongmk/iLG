using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserRole : Entity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}