using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using 充血模型在EFCore中的实现1;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<TestDbContext>(opt => {
    string connStr = "Data Source=.;Initial Catalog=chongxue;Integrated Security=true";
    opt.UseSqlServer(connStr);
});
var sp = services.BuildServiceProvider();
var ctx = sp.GetRequiredService<TestDbContext>();
/*
Dog d1 = new Dog { Name = "goofy" };
Console.WriteLine("Dog初始化完毕");
ctx.Dogs.Add(d1);
ctx.SaveChanges();
Console.WriteLine("SaveChanges完毕");
*/
Console.WriteLine("准备读取数据");
Dog d2 = ctx.Dogs.First(d => d.Name == "goofy");
Console.WriteLine("读取数据完毕");
