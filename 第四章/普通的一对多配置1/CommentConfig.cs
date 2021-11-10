using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

class CommentConfig : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{
		builder.ToTable("T_Comments");
		builder.HasOne<Article>(c => c.Article).WithMany(a => a.Comments)
			.IsRequired().HasForeignKey(c => c.ArticleId);
		builder.Property(c => c.Message).IsRequired().IsUnicode();
	}
}