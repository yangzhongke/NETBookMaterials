using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 多对多1
{
    class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("T_Students");
            builder.Property(s => s.Name).IsRequired().IsUnicode().HasMaxLength(20);
            builder.HasMany<Teacher>(s => s.Teachers).WithMany(t=>t.Students).UsingEntity(j=>j.ToTable("T_Students_Teachers"));
        }
    }
}
