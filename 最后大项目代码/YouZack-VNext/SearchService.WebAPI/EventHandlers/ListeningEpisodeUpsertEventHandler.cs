
using Nest;
using SearchService.WebAPI.IndexModels;
using Zack.EventBus;

namespace SearchService.WebAPI.EventHandlers;

[EventName("ListeningEpisode.Created")]
[EventName("ListeningEpisode.Updated")]
public class ListeningEpisodeUpsertEventHandler : DynamicIntegrationEventHandler
{
    private readonly IElasticClient elasticClient;

    public ListeningEpisodeUpsertEventHandler(IElasticClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        Guid id = Guid.Parse(eventData.Id);
        string cnName = eventData.Name.Chinese;
        string engName = eventData.Name.English;
        Guid albumId = Guid.Parse(eventData.AlbumId);
        List<string> sentences = new List<string>();
        foreach (var sentence in eventData.Sentences)
        {
            sentences.Add(sentence.Value);
        }
        string plainSentences = string.Join("\r\n", sentences);
        Episode episode = new Episode(id, cnName, engName, plainSentences, albumId);
        //设定Id，这样如果遇到同样Id的就更新，而不是插入。Index方法会自动Upsert
        var response = await elasticClient.IndexAsync(episode, idx => idx.Index("episodes").Id(episode.Id));
        if (!response.IsValid)
        {
            throw new ApplicationException(response.DebugInformation);
        }
    }
}
