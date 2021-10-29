
using Listening.Domain.Events;
using Zack.EventBus;

namespace Listening.Admin.WebAPI.EventHandlers;
public class EpisodeUpdatedEventHandler : INotificationHandler<EpisodeUpdatedEvent>
{
    private readonly IEventBus eventBus;

    public EpisodeUpdatedEventHandler(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public Task Handle(EpisodeUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var episode = notification.Value;
        //todo:如果音频地址改变，则重新转码
        //考虑不能修改音频地址，因为主流视频网站也都是这样做的。不过要考虑转码成功后修改AudioUrl的问题，应该的是：新增的时候不是真的新增，而是接入队列，因为这时候的Episode如果插入数据库是非法状态，如果允许非法数据插入数据库，就要对逻辑做复杂处理，所以转码之后才能插入数据库。
        if (episode.IsVisible)
        {
            var sentences = episode.ParseSubtitle();
            eventBus.Publish("ListeningEpisode.Updated", new { Id = episode.Id, episode.Name, Sentences = sentences, episode.AlbumId, episode.Subtitle, episode.SubtitleType });
        }
        else
        {
            //被隐藏
            eventBus.Publish("ListeningEpisode.Hidden", new { Id = episode.Id });
        }
        return Task.CompletedTask;
    }
}
