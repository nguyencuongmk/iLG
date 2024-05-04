using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class UserInfoHobbyConfiguration : IEntityTypeConfiguration<UserInfoHobby>
    {
        public void Configure(EntityTypeBuilder<UserInfoHobby> builder)
        {
            //builder.HasKey(uih => new { uih.UserInfoId, uih.HobbyId });
        }
    }
}