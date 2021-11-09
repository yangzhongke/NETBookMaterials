using Microsoft.Extensions.DependencyInjection;

//普通注册方式
/*
ServiceCollection services = new ServiceCollection();
services.AddTransient<TestServiceImpl>();
using (ServiceProvider sp = services.BuildServiceProvider())
{
    TestServiceImpl testService = sp.GetRequiredService<TestServiceImpl>();
    testService.Name = "tom";
    testService.SayHi();
}*/
ServiceCollection services = new ServiceCollection();
services.AddTransient<TestServiceImpl>();
using (ServiceProvider sp = services.BuildServiceProvider())
{
    var ts1 = sp.GetRequiredService<TestServiceImpl>();
    var ts2 = sp.GetRequiredService<TestServiceImpl>();
    Console.WriteLine(object.ReferenceEquals(ts1, ts2));
}