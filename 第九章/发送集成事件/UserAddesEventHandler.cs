using Zack.EventBus;

[EventName("UserAdded")]
public class UserAddesEventHandler : IIntegrationEventHandler
{
	private readonly ILogger<UserAddesEventHandler> logger;
	public UserAddesEventHandler(ILogger<UserAddesEventHandler> logger)
	{
		this.logger = logger;
	}
	public Task Handle(string eventName, string eventData)
	{
		logger.LogInformation("新建了用户:" + eventData);
		return Task.CompletedTask;
	}
}
