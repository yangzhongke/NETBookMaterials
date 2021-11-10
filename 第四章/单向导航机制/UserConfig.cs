using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

class UserConfig : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("T_Users");
		builder.Property(u => u.Name).IsRequired()
			.HasMaxLength(100).IsUnicode();
	}
}
