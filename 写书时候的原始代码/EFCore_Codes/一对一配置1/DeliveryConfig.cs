using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 一对多配置1
{
    class DeliveryConfig : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("T_Deliveries");
            builder.Property(d => d.CompanyName).IsRequired().IsUnicode().HasMaxLength(10);
            builder.Property(d => d.Number).IsRequired().HasMaxLength(50);
        }
    }
}
