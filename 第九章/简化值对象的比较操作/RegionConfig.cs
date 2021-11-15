using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

class RegionConfig : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.OwnsOne(c => c.Area, nb => {
            nb.Property(e => e.Unit).HasMaxLength(20)
            .IsUnicode(false).HasConversion<string>();
        });
        builder.OwnsOne(c => c.Location);
        builder.Property(c => c.Level).HasMaxLength(20)
            .IsUnicode(false).HasConversion<string>();
        builder.OwnsOne(c => c.Name, nb => {
            nb.Property(e => e.English).HasMaxLength(20).IsUnicode(false);
            nb.Property(e => e.Chinese).HasMaxLength(20).IsUnicode(true);
        });
    }
}