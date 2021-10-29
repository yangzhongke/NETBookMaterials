using Microsoft.Extensions.Configuration;
using System;

namespace Config2_82配置基础2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(
                "config.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configRoot = configBuilder.Build();
            Config config = new Config();
            configRoot.Bind(config);

            Console.WriteLine($"name={config.Name}");
            var proxy = config.Proxy;
            Console.WriteLine($"Address:{proxy.Address},Port:{proxy.Port}");

            Console.WriteLine("******只读取一部分节点******");
            Server server = new Server();
            configRoot.GetSection("proxy").Bind(server);
            Console.WriteLine($"{server.Address},{server.Port}");
            Console.Read();
            Console.WriteLine($"name={config.Name}");
        }
    }
}