using iLG.Domain.Entities;
using iLG.Domain.Enums;
using iLG.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iLG.Infrastructure.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Type).HasConversion(it => it.ToString(), dbType => dbType.ToEnum<ImageType>());
        }
    }
}