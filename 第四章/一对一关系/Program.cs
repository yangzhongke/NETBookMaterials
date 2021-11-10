using Microsoft.EntityFrameworkCore;

using TestDbContext ctx = new TestDbContext();
Order order = new Order();
order.Address = "北京市海淀区中关村南大街999号";
order.Name = "USB充电器";
Delivery delivery = new Delivery();
delivery.CompanyName = "蜗牛快递";
delivery.Number = "SN333322888";
delivery.Order = order;
ctx.Deliveries.Add(delivery);
await ctx.SaveChangesAsync();
Order order1 = await ctx.Orders.Include(o => o.Delivery)
    .FirstAsync(o => o.Name.Contains("充电器"));
Console.WriteLine($"名称：{order1.Name},单号：{order1.Delivery.Number}");