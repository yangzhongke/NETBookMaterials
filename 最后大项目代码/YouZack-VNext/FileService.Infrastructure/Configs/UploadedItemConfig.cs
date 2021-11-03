using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Configs
{
    class UploadedItemConfig : IEntityTypeConfiguration<UploadedItem>
    {
        public void Configure(EntityTypeBuilder<UploadedItem> builder)
        {
            builder.ToTable("T_FS_UploadedItems");
            builder.HasKey(e => e.Id).IsClustered(false);
            builder.Property(e => e.FileName).IsUnicode().HasMaxLength(1024);
            builder.Property(e => e.FileSHA256Hash).IsUnicode(false).HasMaxLength(64);
            builder.HasIndex(e => new { e.FileSHA256Hash, e.FileSizeInBytes });//经常要按照
        }
    }
}
