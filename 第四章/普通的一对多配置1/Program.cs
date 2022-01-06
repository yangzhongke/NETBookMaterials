//插入数据
/*
Article a1 = new Article();
a1.Title = "微软发布.NET 6大版本的首个预览";
a1.Content = "微软昨日在一篇官网博客文章中宣布了 .NET 6 首个预览版本的到来。";
Comment c1 = new Comment() { Message = "支持" };
Comment c2 = new Comment() { Message = "微软太牛了" };
Comment c3 = new Comment() { Message = "火钳刘明" };
a1.Comments.Add(c1);
a1.Comments.Add(c2);
a1.Comments.Add(c3);
using TestDbContext ctx = new TestDbContext();
ctx.Articles.Add(a1);
await ctx.SaveChangesAsync();
*/
using Microsoft.EntityFrameworkCore;
//关联查询
/*
using TestDbContext ctx = new TestDbContext();
Article a = ctx.Articles.Include(a => a.Comments).Single(a => a.Id == 1);
Console.WriteLine(a.Title);
foreach (Comment c in a.Comments)
{
	Console.WriteLine(c.Id + ":" + c.Message);
}*/
//不使用Include的代码

using TestDbContext ctx = new TestDbContext();
Article a = ctx.Articles.Single(a => a.Id == 1);
Console.WriteLine(a.Title);
foreach (Comment c in a.Comments)
{
	Console.WriteLine(c.Id + ":" + c.Message);
}
//实体对象的关联追踪
/*
Article a1 = new Article();
a1.Title = "关于.NET 5正式发布，你应该了解的五件事";
a1.Content = ".NET 5 是 .NET Core 3.1 和 .NET Framework 4.8 的后续产品。";
Comment c1 = new Comment() { Message = "已经在用了", Article = a1 };
Comment c2 = new Comment() { Message = "我们公司项目已经升级到.NET5了", Article = a1 };
using TestDbContext ctx = new TestDbContext();
ctx.Comments.Add(c1);
ctx.Comments.Add(c2);
await ctx.SaveChangesAsync();*/
//只获取外键值
/*
using TestDbContext ctx = new TestDbContext();
foreach (Comment c in ctx.Comments.Include(c => c.Article))
{
	Console.WriteLine(c.Id + ":" + c.Message + "; " + c.Article.Id);
}*/
//只获取外键值2
/*
using TestDbContext ctx = new TestDbContext();
foreach (Comment c in ctx.Comments)
{
	Console.WriteLine(c.Id + ":" + c.Message + "; " + c.ArticleId);
}*/
//查询评论中含有“微软”的文章
/*
TestDbContext ctx = new TestDbContext();
var articles = ctx.Articles.Where(a => a.Comments.Any(c => c.Message.Contains("微软")));
foreach (var article in articles)
{
	Console.WriteLine($"{article.Id},{article.Title}");
}
*/
/*
TestDbContext ctx = new TestDbContext();
var articles = ctx.Comments.Where(c => c.Message.Contains("微软"))
	.Select(c => c.Article).Distinct();
foreach (var article in articles)
{
	Console.WriteLine($"{article.Id},{article.Title}");
}*/