using Zack.EventBus;

[EventName("UserAdded")]
public class UserAddesEventHandler3 : JsonIntegrationEventHandler<UserData>
{
	private readonly ILogger<UserAddesEventHandler3> logger;
	public UserAddesEventHandler3(ILogger<UserAddesEventHandler3> logger)
	{
		this.logger = logger;
	}
	public override Task HandleJson(string eventName, UserData eventData)
	{
		logger.LogInformation($"Json:{eventData.UserName}");
		return Task.CompletedTask;
	}
}
public record UserData(string UserName, int Age);
