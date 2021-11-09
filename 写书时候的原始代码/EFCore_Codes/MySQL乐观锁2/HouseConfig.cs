using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MySQL乐观锁2
{
    class HouseConfig : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder.ToTable("T_Houses2");
            builder.Property(h => h.Name).IsUnicode().IsRequired();
            //builder.Property(h => h.RowVer).HasMaxLength(36).IsConcurrencyToken();
            builder.Property(h => h.RowVer).HasMaxLength(36).HasValueGenerator<GuidValueGenerator>().ValueGeneratedOnUpdate();
        }
    }
}
