using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MySQL乐观锁1
{
    class HouseConfig : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder.ToTable("T_Houses");
            builder.Property(h => h.Name).IsUnicode().IsRequired();
            builder.Property(h => h.Owner).IsConcurrencyToken();
        }
    }
}
