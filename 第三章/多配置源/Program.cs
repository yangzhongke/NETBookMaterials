using Microsoft.Extensions.Configuration;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json")
    .AddEnvironmentVariables("Test1_").AddCommandLine(args);
IConfigurationRoot config = configBuilder.Build();
string server = config["Server"];
string userName = config["UserName"];
string password = config["Password"];
string port = config["Port"];
//Console.WriteLine($"server={server},port={port}");
//Console.WriteLine($"userName={userName},password={password}");
Console.WriteLine(config.GetDebugView());