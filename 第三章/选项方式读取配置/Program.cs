using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfigurationRoot config = configBuilder.Build();
ServiceCollection services = new ServiceCollection();
services.AddOptions()
	.Configure<DbSettings>(e=>config.GetSection("DB").Bind(e))
	.Configure<SmtpSettings>(e => config.GetSection("Smtp").Bind(e));
services.AddTransient<Demo>();
using (var sp = services.BuildServiceProvider())
{
	while (true)
	{
		using (var scope = sp.CreateScope())
		{
			var spScope = scope.ServiceProvider;
			var demo = spScope.GetRequiredService<Demo>();
			demo.Test();
		}
		Console.WriteLine("可以改配置啦");
		Console.ReadKey();
	}
}