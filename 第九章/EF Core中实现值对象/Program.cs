using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ServiceCollection();
services.AddDbContext<TestDbContext>(opt => {
    string connStr = "Data Source=.;Initial Catalog=valueobj1;Integrated Security=true";
    opt.UseSqlServer(connStr);
});

var sp = services.BuildServiceProvider();
var ctx = sp.GetRequiredService<TestDbContext>();
MultilingualString name1 = new MultilingualString("北京", "BeiJing");
Area area1 = new Area(16410, AreaType.SquareKM);
Geo loc = new Geo(116.4074, 39.9042);
Region c1 = new Region(name1, area1, loc, RegionLevel.Province);
c1.ChangePopulation(21893100);
ctx.Cities.Add(c1);
ctx.SaveChanges();
