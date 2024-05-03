using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class RolePermission : Entity
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public Role Role { get; set; }

        public Permission Permission { get; set; }
    }
}