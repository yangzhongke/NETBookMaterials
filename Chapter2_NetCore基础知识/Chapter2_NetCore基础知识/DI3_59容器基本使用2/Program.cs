using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DI3_59容器基本使用2
{
class Program
{
    static void Main(string[] args)
    {
        ServiceCollection services = new ServiceCollection();
        services.AddTransient<FooService>();
        services.AddScoped<BarService>();
            services.TryAddScoped
        using (ServiceProvider sp = services.BuildServiceProvider())
        {
            var fooService = sp.GetService<FooService>();
            using(var scope = sp.CreateScope())
            {
                var spScope = scope.ServiceProvider;
                BarService barService = spScope.GetService<BarService>();
                Console.WriteLine("scope即将结束");
            }
            Console.WriteLine("根Scope即将结束");
        }
        Console.WriteLine("exiting");
    }
}

public class FooService : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine($"释放FooService对象");
    }
}
public class BarService : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine($"释放BarService对象");
    }
}
}
