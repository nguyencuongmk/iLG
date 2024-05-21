using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class HobbyCategoryConfiguration : IEntityTypeConfiguration<HobbyCategory>
    {
        public void Configure(EntityTypeBuilder<HobbyCategory> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Title).HasMaxLength(255).IsRequired();

            // Cấu hình quan hệ của HobbyCategory và Hobby
            builder.HasMany(h => h.Hobbies)
                   .WithOne(hd => hd.HobbyCategory)
                   .HasForeignKey(hd => hd.HobbyCategoryId);
        }
    }
}