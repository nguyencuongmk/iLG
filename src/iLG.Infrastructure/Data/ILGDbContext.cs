using iLG.Domain.Abstractions;
using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace iLG.Infrastructure.Data
{
    public class ILGDbContext : DbContext
    {
        public ILGDbContext(DbContextOptions<ILGDbContext> options) : base(options)
        {
        }

        public DbSet<HobbyCategory> HobbyCategories => Set<HobbyCategory>();

        public DbSet<Hobby> Hobbies => Set<Hobby>();

        public DbSet<Relationship> Relationships => Set<Relationship>();

        public DbSet<Company> Companies => Set<Company>();

        public DbSet<Job> Jobs => Set<Job>();

        public DbSet<Image> Images => Set<Image>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        public DbSet<User> Users => Set<User>();

        public DbSet<UserInfo> UserInfos => Set<UserInfo>();

        public DbSet<UserInfoHobby> UserInfoHobby => Set<UserInfoHobby>();

        public DbSet<UserMatch> UserMatches => Set<UserMatch>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<UserToken> UserTokens => Set<UserToken>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void BeforeSaveChanges()
        {
            var entities = ChangeTracker.Entries();
            var now = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                if (entity.Entity is IEntity baseEntity)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            baseEntity.CreatedAt = now;
                            baseEntity.UpdatedAt = now;
                            break;

                        case EntityState.Modified:
                            baseEntity.UpdatedAt = now;
                            break;
                    }
                }
                else if (entity.Entity is IEntity<int> baseEntityInt)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            baseEntityInt.CreatedAt = now;
                            baseEntityInt.UpdatedAt = now;
                            break;

                        case EntityState.Modified:
                            baseEntityInt.UpdatedAt = now;
                            break;
                    }
                }
            }
        }
    }
}