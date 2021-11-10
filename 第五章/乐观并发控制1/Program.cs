using Microsoft.EntityFrameworkCore;

Console.WriteLine("请输入您的姓名");
string name = Console.ReadLine();
using MyDbContext ctx = new MyDbContext();
var h1 = await ctx.Houses.SingleAsync(h => h.Id == 1);
if (string.IsNullOrEmpty(h1.Owner))
{
	await Task.Delay(5000);
	h1.Owner = name;
	try
	{
		await ctx.SaveChangesAsync();
		Console.WriteLine("抢到手了");
	}
	catch (DbUpdateConcurrencyException ex)
	{
		var entry = ex.Entries.First();
		var dbValues = await entry.GetDatabaseValuesAsync();
		string newOwner = dbValues.GetValue<string>(nameof(House.Owner));
		Console.WriteLine($"并发冲突，被{newOwner}提前抢走了");
	}
}
else
{
	if (h1.Owner == name)
	{
		Console.WriteLine("这个房子已经是你的了，不用抢");
	}
	else
	{
		Console.WriteLine($"这个房子已经被{h1.Owner}抢走了");
	}
}
Console.ReadLine();