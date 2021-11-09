using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 组织结构树1
{
    class OrgUnitConfig : IEntityTypeConfiguration<OrgUnit>
    {
        public void Configure(EntityTypeBuilder<OrgUnit> builder)
        {
            builder.ToTable("T_OrgUnits");
            builder.Property(o => o.Name).IsRequired().IsUnicode().HasMaxLength(100);
            builder.HasOne<OrgUnit>(u => u.Parent).WithMany(p => p.Children);
        }
    }
}
