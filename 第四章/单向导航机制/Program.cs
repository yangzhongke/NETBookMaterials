//插入数据
/*
User u1 = new User { Name = "杨中科" };
Leave leave1 = new Leave();
leave1.Requester = u1;
leave1.From = new DateTime(2021, 8, 8);
leave1.To = new DateTime(2021, 8, 9);
leave1.Remarks = "家里三套房拆迁，回家处理";
leave1.Status = 0;
using TestDbContext ctx = new TestDbContext();
ctx.Users.Add(u1);
ctx.Leaves.Add(leave1);
await ctx.SaveChangesAsync();
*/
using Microsoft.EntityFrameworkCore;

TestDbContext ctx = new TestDbContext();
User u = await ctx.Users.SingleAsync(u => u.Name == "杨中科");
foreach (var l in ctx.Leaves.Where(l => l.Requester == u))
{
	Console.WriteLine(l.Remarks);
}