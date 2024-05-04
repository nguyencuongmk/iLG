using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).HasMaxLength(255).IsRequired();
            builder.Property(r => r.Code).HasMaxLength(10).IsRequired();

            // Cấu hình quan hệ của Role và Permission
            builder.HasMany(r => r.Permissions)
                   .WithMany(p => p.Roles)
                   .UsingEntity<RolePermission>(j => j.HasOne(rp => rp.Permission).WithMany().HasForeignKey(rp => rp.PermissionId), j => j.HasOne(rp => rp.Role).WithMany().HasForeignKey(rp => rp.RoleId), j =>
                    {
                        j.HasKey(rp => new { rp.RoleId, rp.PermissionId });
                        j.ToTable("RolePermissions");
                    });
        }
    }
}