using Microsoft.Extensions.Configuration;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddEnvironmentVariables("TEST_");
IConfigurationRoot configRoot = configBuilder.Build();
string name = configRoot["Name"];
Console.WriteLine(name);