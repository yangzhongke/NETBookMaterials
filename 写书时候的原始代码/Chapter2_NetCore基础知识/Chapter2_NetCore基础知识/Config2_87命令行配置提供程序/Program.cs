using Microsoft.Extensions.Configuration;
using System;

namespace Config2_87命令行配置提供程序
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddCommandLine(args);
            IConfigurationRoot configRoot = configBuilder.Build();
            /*
            string server = config["server"];
            Console.WriteLine($"server:{server}");*/

            Config config = configRoot.Get<Config>();

            Console.WriteLine($"name={config.Name}");
            var proxy = config.Proxy;
            Console.WriteLine($"Address:{proxy.Address},Port:{proxy.Port}");
        }
    }

    class Config
    {
        public string Name { get; set; }
        public Server Proxy { get; set; }
    }

    class Server
    {
        public string Address { get; set; }
        public int Port { get; set; }
    }
}