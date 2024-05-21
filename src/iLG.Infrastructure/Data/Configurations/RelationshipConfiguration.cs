using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    internal class RelationshipConfiguration : IEntityTypeConfiguration<Relationship>
    {
        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.UserInfos)
                .WithOne(u => u.Relationship)
                .HasForeignKey(u => u.RelationshipId);
        }
    }
}