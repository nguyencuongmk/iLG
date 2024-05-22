using iLG.Domain.Entities;
using iLG.Infrastructure.Data;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace iLG.Infrastructure.Repositories
{
    public class PermissionRepository(ILGDbContext context) : Repository<Permission>(context), IPermissionRepository
    {
        private readonly ILGDbContext _context = context;

        public async Task<List<Permission>> GetPermissionsByRoles(List<Role>? roles)
        {
            if (roles is null || roles.Count == 0)
                return [];

            var userPermissions = new List<Permission>();

            foreach (var role in roles)
            {
                var roleId = role.Id;
                var permissions = await _context.Permissions.Where(p => _context.RolePermissions.Any(rp => rp.RoleId == roleId && rp.PermissionId == p.Id)).ToListAsync();

                if (permissions is not null && permissions.Count != 0)
                    userPermissions.AddRange(permissions);
            }

            return userPermissions.Distinct().ToList();
        }
    }
}