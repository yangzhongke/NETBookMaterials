using System.Text;

public class ExplortStatisticBgService : BackgroundService
{
	private readonly TestDbContext ctx;
	private readonly ILogger<ExplortStatisticBgService> logger;
	private readonly IServiceScope serviceScope;
	public ExplortStatisticBgService(IServiceScopeFactory scopeFactory)
	{
		this.serviceScope = scopeFactory.CreateScope();
		var sp = serviceScope.ServiceProvider;
		this.ctx = sp.GetRequiredService<TestDbContext>();
		this.logger = sp.GetRequiredService<ILogger<ExplortStatisticBgService>>();
	}
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await DoExecuteAsync();
				await Task.Delay(5000);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "获取用户统计数据失败");
				await Task.Delay(1000);
			}
		}
	}
	private async Task DoExecuteAsync()
	{
		var items = ctx.Users.GroupBy(u => u.CreationTime.Date)
						.Select(e => new { Date = e.Key, Count = e.Count() });
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"Date:{DateTime.Now}");
		foreach (var item in items)
		{
			sb.Append(item.Date).AppendLine($":{item.Count}");
		}
		await File.WriteAllTextAsync("d:/1.txt", sb.ToString());
		logger.LogInformation($"导出完成");
	}
	public override void Dispose()
	{
		base.Dispose();
		serviceScope.Dispose();
	}
}