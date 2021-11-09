using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using 一对多配置1;

namespace 组织结构树1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                /*
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
                Console.WriteLine($"商品名称：{order1.Name},快递单号：{order1.Delivery.Number}");*/
                var orders = ctx.Orders.Where(o=>o.Delivery.CompanyName== "蜗牛快递");
                foreach(var order in orders)
                {
                    Console.WriteLine(order.Name);
                }
            }
        }
    }
}
