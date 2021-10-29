
using Nest;
using SearchService.WebAPI.IndexModels;
using Zack.EventBus;

namespace SearchService.WebAPI.EventHandlers;

[EventName("ListeningEpisode.Deleted")]
[EventName("ListeningEpisode.Hidden")]//被隐藏也看作删除
public class ListeningEpisodeDeletedEventHandler : DynamicIntegrationEventHandler
{
    private readonly IElasticClient elasticClient;

    public ListeningEpisodeDeletedEventHandler(IElasticClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        Guid id = Guid.Parse(eventData.Id);
        elasticClient.DeleteByQuery<Episode>(q => q
            .Index("episodes")
            .Query(rq => rq.Term(f => f.Id, "elasticsearch.pm"))
        );
        //因为有可能文档不存在，所以不检查结果
        return elasticClient.DeleteAsync(new DeleteRequest("episodes", id));
    }
}
