using Microsoft.EntityFrameworkCore;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<TestDbContext>(opt => {
    string connStr = "Data Source=.;Initial Catalog=chongxue;Integrated Security=true";
    opt.UseSqlServer(connStr);
});

var sp = services.BuildServiceProvider();
var ctx = sp.GetRequiredService<TestDbContext>();
/*
Dog d1 = new Dog { Name= "goofy" };
Console.WriteLine("Dog初始化完毕");
ctx.Dogs.Add(d1);
ctx.SaveChanges();
Console.WriteLine("SaveChanges完毕");*/
/*
Console.WriteLine("准备读取数据");
Dog d2 = ctx.Dogs.First(d=>d.Name== "goofy");
Console.WriteLine("读取数据完毕");*/
/*
User u1 = new User("Zack");
u1.Tag = "MyTag";
u1.ChangePassword("123456");
ctx.Users.Add(u1);
ctx.SaveChanges();*/
User u1 = ctx.Users.First(u=>u.UserName=="Zack");
Console.WriteLine(u1);