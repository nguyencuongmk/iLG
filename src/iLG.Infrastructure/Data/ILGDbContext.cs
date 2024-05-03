using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace iLG.Infrastructure.Data
{
    public class ILGDbContext : DbContext
    {
        public ILGDbContext(DbContextOptions<ILGDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<User> Users => Set<User>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}