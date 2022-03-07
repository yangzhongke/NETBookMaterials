using SearchService.Domain;
using Zack.EventBus;

namespace SearchService.WebAPI.EventHandlers;

[EventName("ListeningEpisode.Created")]
[EventName("ListeningEpisode.Updated")]
public class ListeningEpisodeUpsertEventHandler : DynamicIntegrationEventHandler
{
    private readonly ISearchRepository repository;

    public ListeningEpisodeUpsertEventHandler(ISearchRepository repository)
    {
        this.repository = repository;
    }

    public override Task HandleDynamic(string eventName, dynamic eventData)
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
        return repository.UpsertAsync(episode);
    }
}
