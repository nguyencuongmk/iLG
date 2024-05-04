using iLG.Domain.Entities;
using iLG.Domain.Enums;
using iLG.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasKey(ui => ui.Id);
            builder.Property(ui => ui.FullName).HasMaxLength(255).IsRequired();
            builder.Property(ui => ui.Gender).HasConversion(g => g.ToString(), dbGender => dbGender.ToEnum<Gender>());
            builder.Property(ui => ui.Zodiac).HasConversion(z => z.ToString(), dbZodiac => dbZodiac.ToEnum<Zodiac>());

            // Cấu hình quan hệ của Image và UserInfo
            builder.HasMany(ui => ui.Images)
                   .WithOne(i => i.UserInfo)
                   .HasForeignKey(i => i.UserInfoId);

            // Cấu hình quan hệ của UserInfo và Hobby
            builder.HasMany(ui => ui.Hobbies)
                   .WithMany(h => h.UserInfos)
                   .UsingEntity<UserInfoHobby>(j => j.HasOne(uh => uh.Hobby).WithMany().HasForeignKey(uh => uh.HobbyId), j => j.HasOne(uh => uh.UserInfo).WithMany().HasForeignKey(uh => uh.UserInfoId), j =>
                     {
                         j.HasKey(uh => new { uh.UserInfoId, uh.HobbyId });
                         j.ToTable("UserInfoHobbies");
                     });

            // Cấu hình quan hệ của UserInfo và UserMatch
            builder.HasMany(u => u.UserMatches)
                   .WithOne(m => m.UserInfo)
                   .HasForeignKey(m => m.UserInfoId);
        }
    }
}