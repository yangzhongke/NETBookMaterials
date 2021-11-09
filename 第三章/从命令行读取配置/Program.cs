using Microsoft.Extensions.Configuration;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddCommandLine(args);
IConfigurationRoot config = configBuilder.Build();
string server = config["server"];
Console.WriteLine($"server:{server}");