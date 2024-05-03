using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class Permission : Entity<int>
    {
        public string? Name { get; set; }

        public virtual List<Role> Roles { get; set; } = [];
    }
}