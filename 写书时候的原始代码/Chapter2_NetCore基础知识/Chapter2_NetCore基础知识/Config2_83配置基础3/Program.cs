using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Config2_83配置基础3
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(
                "appsettings.json", optional: false, reloadOnChange: true);
            IConfigurationRoot config = configBuilder.Build();
            ServiceCollection services = new ServiceCollection();
            services.AddOptions()
                .Configure<DbSettings>(e=>config.GetSection("DB").Bind(e))
                .Configure<SmtpSettings>(e => config.GetSection("Smtp").Bind(e));
            services.AddTransient<Demo>();//要用Scoped或Transient，不能用Singleton
            using (var sp = services.BuildServiceProvider())
            {
                while (true)
                {
                    //同一个Scope中配置改变不会刷新
                    using (var scope = sp.CreateScope())
                    {
                        var spScope = scope.ServiceProvider;
                        var demo = spScope.GetService<Demo>();
                        demo.Test();
                    }
                    Console.WriteLine("可以改配置啦");
                    Console.ReadKey();
                }
            }
        }
    }
}