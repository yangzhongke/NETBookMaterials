using Microsoft.Extensions.Configuration;
using System;

namespace Config2_89环境变量配置提供程序
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddEnvironmentVariables("TEST_");
            //configBuilder.AddEnvironmentVariables();
            IConfigurationRoot configRoot = configBuilder.Build();
            string name = configRoot["Name"];
            Console.WriteLine(name);
        }
    }
}
