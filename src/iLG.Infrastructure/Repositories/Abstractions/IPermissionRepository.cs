using iLG.Domain.Entities;

namespace iLG.Infrastructure.Repositories.Abstractions
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<List<Permission>> GetPermissionsByRoles(List<Role>? roles);
    }
}