using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace 一对多配置1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
            Article a1 = new Article();
            a1.Title = "微软发布.NET 6大版本的首个预览";
            a1.Content = "微软昨日在一篇官网博客文章中宣布了 .NET 6 首个预览版本的到来，可知本次大版本更新带来了诸多的新特性。包括云、桌面、以及移动应用程序，都将在 .NET 6 时代迎来重大的改进。与此同时，微软正在将 Xamarin 的 Android / iOS / macOS 部分功能，也集成到 .NET 6 中。";
            Comment c1 = new Comment() { Message="支持"};
            Comment c2 = new Comment() { Message = "微软太牛了" };
            Comment c3 = new Comment() { Message = "火钳刘明" };
            a1.Comments.Add(c1);
            a1.Comments.Add(c2);
            a1.Comments.Add(c3);

            using (TestDbContext ctx = new TestDbContext())
            {
                ctx.Articles.Add(a1);
                await ctx.SaveChangesAsync();
            }*/

            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                //Article a =  ctx.Articles.Include(a=>a.Comments).Single(a=>a.Id==1);
                Article a = ctx.Articles.Single(a => a.Id == 1);
                Console.WriteLine(a.Title);
                foreach(Comment c in a.Comments)
                {
                    Console.WriteLine(c.Id+":"+c.Message);
                }
            }*/

            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                Comment c = ctx.Comments.Include(c=>c.Article).First(c=>c.Message.Contains("牛"));
                Console.WriteLine(c.Id+":"+c.Message+"; "+c.Article.Id);
                Console.WriteLine();
            }*/
            /*
            Article a1 = new Article();
            a1.Title = "66关于.NET 5正式发布，你应该了解的五件事";
            a1.Content = "66.NET 5 是 .NET Core 3.1 和 .NET Framework 4.8 的后续产品，旨在为 .NET 开发人员提供新的跨平台开发体验。";
            Comment c1 = new Comment() { Message = "66已经在用了",Article=a1 };
            Comment c2 = new Comment() { Message = "66我们公司项目已经升级到.NET5了", Article = a1 };   
            using (TestDbContext ctx = new TestDbContext())
            {
                ctx.Comments.Add(c1);
                ctx.Comments.Add(c2);
                await ctx.SaveChangesAsync();
            }*/
            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                foreach(Comment c in ctx.Comments.Include(c => c.Article))
                {
                    Console.WriteLine(c.Id + ":" + c.Message + "; " + c.Article.Id);
                }
            }*/

            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                foreach (Comment c in ctx.Comments)
                {
                    Console.WriteLine(c.Id + ":" + c.Message + "; " + c.ArticleId);
                }
            }*/

            /*
            User u1 = new User { Name="杨中科"};
            Leave leave1 = new Leave();
            leave1.Requester = u1;
            leave1.From = new DateTime(2021, 8, 8);
            leave1.To = new DateTime(2021, 8, 9);
            leave1.Remarks = "家里三套房拆迁，回家处理";
            leave1.Status = 0;

            Leave leave2 = new Leave();
            leave2.Requester = u1;
            leave2.From = new DateTime(2021, 8, 10);
            leave2.To = new DateTime(2021, 8, 11);
            leave2.Remarks = "去银行存钱";
            leave2.Status = 0;

            using (TestDbContext ctx = new TestDbContext())
            {
                ctx.Users.Add(u1);
                ctx.Leaves.Add(leave1);
                ctx.Leaves.Add(leave2);
                await ctx.SaveChangesAsync();
            }*/
            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                User u = await ctx.Users.SingleAsync(u=>u.Name=="杨中科");
                foreach(var l in ctx.Leaves.Where(l => l.Requester == u))
                {
                    Console.WriteLine(l.Remarks);
                }
            }*/
            /*
         using (TestDbContext ctx = new TestDbContext())
         {

             var articles = ctx.Articles.Where(a=>a.Comments.Any(c=>c.Message.Contains("微软")));
             foreach(var article in articles)
             {
                 Console.WriteLine($"{article.Id},{article.Title}");
             }
            var articles = ctx.Comments.Where(c => c.Message.Contains("微软"))
                    .Select(c => c.Article).Distinct();
                foreach (var article in articles)
                {
                    Console.WriteLine($"{article.Id},{article.Title}");
                }
            }*/
            
        }
    }
}
