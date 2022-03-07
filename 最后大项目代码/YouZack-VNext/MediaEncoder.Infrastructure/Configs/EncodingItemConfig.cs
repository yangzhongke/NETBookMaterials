using MediaEncoder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaEncoder.Infrastructure.Configs
{
    class EncodingItemConfig : IEntityTypeConfiguration<EncodingItem>
    {
        public void Configure(EntityTypeBuilder<EncodingItem> builder)
        {
            builder.ToTable("T_ME_EncodingItems");
            //todo:id需要非聚集索引。
            //todo:符合索引。
            builder.Property(e => e.Name).HasMaxLength(256);
            builder.Property(e => e.FileSHA256Hash).HasMaxLength(64).IsUnicode(false);
            builder.Property(e => e.OutputFormat).HasMaxLength(10).IsUnicode(false);
            builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(10);
        }
    }
}
