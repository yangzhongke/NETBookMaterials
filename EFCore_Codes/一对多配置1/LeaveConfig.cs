using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 一对多配置1
{
    class LeaveConfig : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.ToTable("T_Leaves");
            builder.HasOne<User>(l => l.Requester).WithMany().IsRequired();
            builder.HasOne<User>(l => l.Approver).WithMany();
            builder.Property(l => l.Remarks).IsRequired()
                .HasMaxLength(1000).IsUnicode();
        }
    }
}
