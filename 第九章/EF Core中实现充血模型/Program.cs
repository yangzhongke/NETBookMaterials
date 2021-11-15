using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<TestDbContext>(opt => {
    string connStr = "Data Source=.;Initial Catalog=chongxue;Integrated Security=true";
    opt.UseSqlServer(connStr);
});
var sp = services.BuildServiceProvider();
var ctx = sp.GetRequiredService<TestDbContext>();
User u1 = new User("Zack");
u1.Tag = "MyTag";
u1.ChangePassword("123456");
ctx.Users.Add(u1);
ctx.SaveChanges();
