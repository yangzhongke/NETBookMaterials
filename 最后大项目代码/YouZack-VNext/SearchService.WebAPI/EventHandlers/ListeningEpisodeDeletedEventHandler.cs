using SearchService.Domain;
using Zack.EventBus;

namespace SearchService.WebAPI.EventHandlers;

[EventName("ListeningEpisode.Deleted")]
[EventName("ListeningEpisode.Hidden")]//被隐藏也看作删除
public class ListeningEpisodeDeletedEventHandler : DynamicIntegrationEventHandler
{
    private readonly ISearchRepository repository;

    public ListeningEpisodeDeletedEventHandler(ISearchRepository repository)
    {
        this.repository = repository;
    }

    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        Guid id = Guid.Parse(eventData.Id);
        return repository.DeleteAsync(id);
    }
}
