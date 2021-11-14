public class DemoBgService : BackgroundService
{
	private ILogger<DemoBgService> logger;
	public DemoBgService(ILogger<DemoBgService> logger)
	{
		this.logger = logger;
	}
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Delay(5000);
		string s = await File.ReadAllTextAsync("d:/1.txt");
		await Task.Delay(20000);
		logger.LogInformation(s);
	}
}
