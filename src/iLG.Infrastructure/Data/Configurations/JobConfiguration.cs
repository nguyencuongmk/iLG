using iLG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    internal class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(j => j.UserInfos)
                .WithOne(ui => ui.Job)
                .HasForeignKey(ui => ui.JobId);
        }
    }
}