using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<TestDbContext>(opt => {
    string connStr = "Data Source=.;Initial Catalog=valueobj1;Integrated Security=true";
    opt.UseSqlServer(connStr);
});

var sp = services.BuildServiceProvider();
var ctx = sp.GetRequiredService<TestDbContext>();
var cities = ctx.Cities.Where(ExpressionHelper.MakeEqual((Region c) => c.Name, 
    new MultilingualString("北京", "BeiJing")));
foreach(var c in cities)
{
    Console.WriteLine(c);
}