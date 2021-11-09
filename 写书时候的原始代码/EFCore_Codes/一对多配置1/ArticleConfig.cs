using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 一对多配置1
{
    class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("T_Articles");
            builder.HasMany<Comment>(a => a.Comments).WithOne(c => c.Article).IsRequired();
            builder.Property(a => a.Content).IsRequired().IsUnicode();
            builder.Property(a => a.Title).IsRequired().IsUnicode().HasMaxLength(255);
        }
    }
}
