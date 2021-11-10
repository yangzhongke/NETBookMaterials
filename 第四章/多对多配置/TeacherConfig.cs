using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

class TeacherConfig : IEntityTypeConfiguration<Teacher>
{
	public void Configure(EntityTypeBuilder<Teacher> builder)
	{
		builder.ToTable("T_Teachers");
		builder.Property(s => s.Name).IsUnicode().HasMaxLength(20);
	}
}