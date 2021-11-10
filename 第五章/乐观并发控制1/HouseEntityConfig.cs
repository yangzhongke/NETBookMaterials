using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

class HouseConfig : IEntityTypeConfiguration<House>
{
	public void Configure(EntityTypeBuilder<House> builder)
	{
		builder.ToTable("T_Houses");
		builder.Property(h => h.Name).IsUnicode();
		builder.Property(h => h.Owner).IsConcurrencyToken();
	}
}
