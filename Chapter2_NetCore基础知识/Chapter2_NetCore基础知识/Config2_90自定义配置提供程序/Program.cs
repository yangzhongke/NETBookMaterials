using Microsoft.Extensions.Configuration;
using System;

namespace Config2_90自定义配置提供程序
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddFxConfigFile("Web.config");
            IConfigurationRoot configRoot = configBuilder.Build();
            AppConfig appConfig = configRoot.Get<AppConfig>();
            Console.WriteLine($"Redis:{appConfig.RedisServer},{appConfig.RedisPassword}");
            var connStr1 = appConfig.ConnStr1;
            Console.WriteLine($"ConnStr1:{connStr1.ConnectionString},{connStr1.ProviderName}");
            var smtp = appConfig.Smtp;
            Console.WriteLine($"Smtp:{smtp.Server},{smtp.Port}");
        }
    }

    public class AppConfig
    {
        public string RedisPassword { get; set; }
        public string RedisServer { get; set; }
        public ConnectionSettings ConnStr1 { get; set; }
        public SmtpSettings Smtp { get; set; }
    }

    public class ConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }

    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
    }
}
