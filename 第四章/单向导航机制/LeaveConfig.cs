using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

class LeaveConfig : IEntityTypeConfiguration<Leave>
{
	public void Configure(EntityTypeBuilder<Leave> builder)
	{
		builder.ToTable("T_Leaves");
		builder.HasOne<User>(l => l.Requester).WithMany();
		builder.HasOne<User>(l => l.Approver).WithMany();
		builder.Property(l => l.Remarks).HasMaxLength(1000).IsUnicode();
	}
}
