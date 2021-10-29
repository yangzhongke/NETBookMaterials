using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 一对多配置1
{
    class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("T_Orders");
            builder.Property(o => o.Address).IsRequired().IsUnicode();
            builder.Property(o=>o.Name).IsRequired().IsUnicode();
            builder.HasOne<Delivery>(o => o.Delivery).WithOne(d => d.Order).HasForeignKey<Delivery>(d=>d.OrderId);
        }
    }
}
