using Zack.EventBus;

[EventName("UserAdded")]
public class UserAddesEventHandler2 : DynamicIntegrationEventHandler
{
    private readonly ILogger<UserAddesEventHandler2> logger;
    public UserAddesEventHandler2(ILogger<UserAddesEventHandler2> logger)
    {
        this.logger = logger;
    }
    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        logger.LogInformation($"Dynamic:{eventData.UserName}");
        return Task.CompletedTask;
    }
}
