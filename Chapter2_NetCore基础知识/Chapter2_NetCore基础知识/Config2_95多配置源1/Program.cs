using Microsoft.Extensions.Configuration;
using System;

namespace Config2_95多配置源1
{
    class Program
    {
        static void Main(string[] args)
        {
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json")
    .AddEnvironmentVariables("Test1_")
    .AddCommandLine(args);
IConfigurationRoot config = configBuilder.Build();
string server = config["Server"];
string userName = config["UserName"];
string password = config["Password"];
string port = config["Port"];
            
            /*
Console.WriteLine($"server={server}");
Console.WriteLine($"userName={userName}");
Console.WriteLine($"password={password}");
Console.WriteLine($"port={port}");*/
            Console.WriteLine(config.GetDebugView());

        }
    }
}
