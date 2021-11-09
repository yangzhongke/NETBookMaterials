using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DI3_51容器基本使用1
{
    class Program
    {
        static void Main(string[] args)
        {
            //3-52
            /*
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<TestServiceImpl>();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                TestServiceImpl testService = serviceProvider.GetService<TestServiceImpl>();
                //ITestService testService = serviceProvider.GetService<ITestService>();
                testService.Name = "tom";
                testService.SayHi();
            }*/

            /*
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<TestServiceImpl>();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var ts1 = serviceProvider.GetService<TestServiceImpl>();
                var ts2 = serviceProvider.GetService<TestServiceImpl>();
                Console.WriteLine(object.ReferenceEquals(ts1,ts2));
            }*/

            //2 54
            /*
            ServiceCollection services = new ServiceCollection();
services.AddScoped<TestServiceImpl>();
using (ServiceProvider serviceProvider = services.BuildServiceProvider())
{
    TestServiceImpl ts1;
    TestServiceImpl ts3;
    using (IServiceScope scope1 = serviceProvider.CreateScope())
    {
        IServiceProvider sp1 = scope1.ServiceProvider;
        ts1 = sp1.GetService<TestServiceImpl>();
        TestServiceImpl ts2 = sp1.GetService<TestServiceImpl>();
        Console.WriteLine(object.ReferenceEquals(ts1, ts2));
    }
    using (IServiceScope scope2 = serviceProvider.CreateScope())
    {
        IServiceProvider sp2 = scope2.ServiceProvider;
        ts3 = sp2.GetService<TestServiceImpl>();
        TestServiceImpl ts4 = sp2.GetService<TestServiceImpl>();
        Console.WriteLine(object.ReferenceEquals(ts3, ts4));
    }
    Console.WriteLine(object.ReferenceEquals(ts1, ts3));

}*/

            //2 55
            /*
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<TestServiceImpl>();
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                TestServiceImpl ts1;
                TestServiceImpl ts2;
                using (IServiceScope scope1 = serviceProvider.CreateScope())
                {
                    IServiceProvider sp1 = scope1.ServiceProvider;
                    ts1 = sp1.GetService<TestServiceImpl>();
                }
                using (IServiceScope scope2 = serviceProvider.CreateScope())
                {
                    IServiceProvider sp2 = scope2.ServiceProvider;
                    ts2 = sp2.GetService<TestServiceImpl>();
                }
                Console.WriteLine(object.ReferenceEquals(ts1, ts2));
            }*/

            //2-56
            /*
            ServiceCollection services = new ServiceCollection();
services.AddTransient<TestServiceImpl>();
services.AddTransient(typeof(TestServiceImpl));
services.AddTransient<ITestService,TestServiceImpl>();
services.AddTransient(typeof(ITestService), typeof(TestServiceImpl));
services.AddTransient<ITestService, TestServiceImpl>(sp=> {
    TestServiceImpl impl = new TestServiceImpl();
    impl.Name = "默认值";
    return impl;
});
services.AddTransient<ITestService>(sp => {
    TestServiceImpl impl = new TestServiceImpl();
    impl.Name = "默认值";
    return impl;
});
services.AddTransient(typeof(ITestService), sp => {
    TestServiceImpl impl = new TestServiceImpl();
    impl.Name = "默认值";
    return impl;
});*/

            /*
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<ITestService>(sp => {
                TestServiceImpl impl = new TestServiceImpl();
                impl.Name = "默认值";
                return impl;
            }); ;
            using (ServiceProvider sp = services.BuildServiceProvider())
            {
                ITestService test = sp.GetService<ITestService>();
                test.SayHi();
            }*/
            /*
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<ITestService,TestServiceImpl>();
            services.AddTransient<ITestService, TestServiceImpl2>();
            using (ServiceProvider sp = services.BuildServiceProvider())
            {
                Console.WriteLine("-----------GetService-----------");
                ITestService test1 = sp.GetService<ITestService>();
                test1.Name = "tom";
                test1.SayHi();

                Console.WriteLine("-----------GetServices-----------");
                IEnumerable<ITestService> tests = sp.GetServices<ITestService>();
                foreach(var test in tests)
                {
                    test.Name = "jerry";
                    test.SayHi();
                }
            }*/
        }
    }
}