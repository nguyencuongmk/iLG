using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class User : Entity<int>
    {
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public string PasswordHash { get; set; }

        public bool IsLocked { get; set; } = false;

        public virtual UserInfo UserInfo { get; set; }

        public virtual List<Role> Roles { get; set; } = [];
    }
}