using Zack.EventBus;

namespace Listening.Admin.WebAPI.EventHandlers
{
    [EventName("SearchService.ReIndexAll")]
    //让搜索引擎服务器，重新收录所有的Episode
    public class ReIndexAllEventHandler : IIntegrationEventHandler
    {
        private readonly ListeningDbContext dbContext;
        private readonly IEventBus eventBus;

        public ReIndexAllEventHandler(ListeningDbContext dbContext, IEventBus eventBus)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
        }

        public Task Handle(string eventName, string eventData)
        {
            foreach (var episode in dbContext.Query<Episode>())
            {
                if (episode.IsVisible)
                {
                    var sentences = episode.ParseSubtitle();
                    eventBus.Publish("ListeningEpisode.Updated", new { Id = episode.Id, episode.Name, Sentences = sentences, episode.AlbumId, episode.Subtitle, episode.SubtitleType });
                }
            }
            return Task.CompletedTask;
        }
    }
}
