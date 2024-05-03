using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class Role : Entity<int>
    {
        public string? Name { get; set; }

        public string? Code { get; set; }

        public virtual List<User> Users { get; set; } = [];

        public virtual List<Permission> Permissions { get; set; } = [];
    }
}