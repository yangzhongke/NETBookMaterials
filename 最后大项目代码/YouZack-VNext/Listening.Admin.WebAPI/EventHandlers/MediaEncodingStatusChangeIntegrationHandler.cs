using Listening.Admin.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Zack.EventBus;

namespace Listening.Admin.WebAPI.EventHandlers;

[EventName("MediaEncoding.Started")]
[EventName("MediaEncoding.Failed")]
[EventName("MediaEncoding.Duplicated")]
[EventName("MediaEncoding.Completed")]
class MediaEncodingStatusChangeIntegrationHandler : DynamicIntegrationEventHandler
{
    private readonly ListeningDbContext dbContext;
    private readonly IListeningRepository repository;
    private readonly EncodingEpisodeHelper encHelper;
    private readonly IHubContext<EpisodeEncodingStatusHub> hubContext;

    public MediaEncodingStatusChangeIntegrationHandler(ListeningDbContext dbContext, 
        EncodingEpisodeHelper encHelper,
        IHubContext<EpisodeEncodingStatusHub> hubContext, IListeningRepository repository)
    {
        this.dbContext = dbContext;
        this.encHelper = encHelper;
        this.hubContext = hubContext;
        this.repository = repository;
    }

    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        string sourceSystem = eventData.SourceSystem;
        if (sourceSystem != "Listening")//可能是别的系统的转码消息
        {
            return;
        }
        Guid id = Guid.Parse(eventData.Id);//EncodingItem的Id就是Episode 的Id

        switch (eventName)
        {
            case "MediaEncoding.Started":
                await encHelper.UpdateEpisodeStatusAsync(id, "Started");
                await hubContext.Clients.All.SendAsync("OnMediaEncodingStarted", id);//通知前端刷新
                break;
            case "MediaEncoding.Failed":
                await encHelper.UpdateEpisodeStatusAsync(id, "Failed");
                //todo: 这样做有问题，这样就会把消息发送给所有打开这个界面的人，应该用connectionId、userId等进行过滤，
                await hubContext.Clients.All.SendAsync("OnMediaEncodingFailed", id);
                break;
            case "MediaEncoding.Duplicated":
                await encHelper.UpdateEpisodeStatusAsync(id, "Completed");
                await hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", id);//通知前端刷新
                break;
            case "MediaEncoding.Completed":
                await encHelper.UpdateEpisodeStatusAsync(id, "Completed");
                Uri outputUrl = new Uri(eventData.OutputUrl);
                var encItem = await encHelper.GetEncodingEpisodeAsync(id);

                Guid albumId = encItem.AlbumId;
                int maxSeq = await repository.GetMaxSeqOfEpisodesAsync(albumId);
                /*
                Episode episode = Episode.Create(id, maxSeq.Value + 1, encodingEpisode.Name, albumId, outputUrl,
                    encodingEpisode.DurationInSecond, encodingEpisode.SubtitleType, encodingEpisode.Subtitle);*/
                var builder = new Episode.Builder();
                builder.Id(id).SequenceNumber(maxSeq+1).Name(encItem.Name)
                    .AlbumId(albumId).AudioUrl(outputUrl)
                    .DurationInSecond(encItem.DurationInSecond)
                    .SubtitleType(encItem.SubtitleType).Subtitle(encItem.Subtitle);
                dbContext.Add(builder.Build());
                await dbContext.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", id);//通知前端刷新
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventName));
        }
    }
}
