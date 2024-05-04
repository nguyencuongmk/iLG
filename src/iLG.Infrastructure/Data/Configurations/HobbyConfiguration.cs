using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class HobbyConfiguration : IEntityTypeConfiguration<Hobby>
    {
        public void Configure(EntityTypeBuilder<Hobby> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Title).HasMaxLength(255).IsRequired();

            // Cấu hình quan hệ của Hobby và HobbyDetail
            builder.HasMany(h => h.HobbyDetails)
                   .WithOne(hd => hd.Hobby)
                   .HasForeignKey(hd => hd.HobbyId);
        }
    }
}