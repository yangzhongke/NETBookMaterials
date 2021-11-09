using Microsoft.Extensions.Configuration;

//手动读取配置
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("config.json", optional: false, reloadOnChange: false);
IConfigurationRoot config = configBuilder.Build();
string name = config["name"];
Console.WriteLine($"name={name}");
string proxyAddress = config.GetSection("proxy:address").Value;
Console.WriteLine($"Address:{proxyAddress}");