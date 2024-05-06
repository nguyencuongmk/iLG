using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.PasswordHash).IsRequired();

            // User and Role relationship configuration
            builder.HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<UserRole>(j => j.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId), j => j.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId), j =>
                    {
                        j.HasKey(ur => new { ur.UserId, ur.RoleId });
                        j.ToTable("UserRoles");
                    });

            // User and UserToken relationship configuration
            builder.HasMany(u => u.UserTokens)
                   .WithOne(ut => ut.User)
                   .HasForeignKey(ut => ut.UserId);
        }
    }
}