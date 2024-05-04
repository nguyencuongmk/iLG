using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class HobbyDetailConfiguration : IEntityTypeConfiguration<HobbyDetail>
    {
        public void Configure(EntityTypeBuilder<HobbyDetail> builder)
        {
            builder.HasKey(hd => hd.Id);
            builder.Property(hd => hd.Name).HasMaxLength(255).IsRequired();
        }
    }
}