using Microsoft.Extensions.Configuration;
using System;

namespace Config2_81_配置基础1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(
                "config.json", optional: false, reloadOnChange: false);
            IConfigurationRoot config = configBuilder.Build();
            string name = config["name"];
            Console.WriteLine($"name={name}");
            string proxyAddress = config.GetSection("proxy:address").Value;
            Console.WriteLine($"Address:{proxyAddress}");
        }
    }
}