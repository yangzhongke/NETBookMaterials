using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 一对多配置1
{
    class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comments");
            //这里的a=>a.Article不能省略，因为实体可能有多个属性指向同一个类型的实体
            /*
            builder.HasOne<Article>(c=>c.Article).WithMany(a => a.Comments).IsRequired().HasForeignKey(c=>c.ArticleId);*/
            builder.Property(c=>c.Message).IsRequired().IsUnicode();
        }
    }
}
