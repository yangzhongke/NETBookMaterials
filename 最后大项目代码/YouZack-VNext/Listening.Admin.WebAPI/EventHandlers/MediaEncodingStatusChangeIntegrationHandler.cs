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
    private readonly EncodingEpisodeHelper encodingEpisodeHelper;
    private readonly IHubContext<EpisodeEncodingStatusHub> hubContext;

    public MediaEncodingStatusChangeIntegrationHandler(ListeningDbContext dbContext, EncodingEpisodeHelper encodingEpisodeHelper,
        IHubContext<EpisodeEncodingStatusHub> hubContext)
    {
        this.dbContext = dbContext;
        this.encodingEpisodeHelper = encodingEpisodeHelper;
        this.hubContext = hubContext;
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
                await encodingEpisodeHelper.UpdateEpisodeStatusAsync(id, "Started");
                await hubContext.Clients.All.SendAsync("OnMediaEncodingStarted", id);//通知前端刷新
                break;
            case "MediaEncoding.Failed":
                await encodingEpisodeHelper.UpdateEpisodeStatusAsync(id, "Failed");
                //todo: 这样做有问题，这样就会把消息发送给所有打开这个界面的人，应该用connectionId、userId等进行过滤，
                await hubContext.Clients.All.SendAsync("OnMediaEncodingFailed", id);
                break;
            case "MediaEncoding.Duplicated":
                await encodingEpisodeHelper.UpdateEpisodeStatusAsync(id, "Completed");
                await hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", id);//通知前端刷新
                break;
            case "MediaEncoding.Completed":
                await encodingEpisodeHelper.UpdateEpisodeStatusAsync(id, "Completed");
                Uri outputUrl = new Uri(eventData.OutputUrl);
                var encItem = await encodingEpisodeHelper.GetEncodingEpisodeAsync(id);

                Guid albumId = encItem.AlbumId;
                int? maxSeq = await dbContext.Query<Episode>().Where(e => e.AlbumId == albumId)
                        .MaxAsync(e => (int?)e.SequenceNumber);
                maxSeq = maxSeq ?? 0;
                /*
                Episode episode = Episode.Create(id, maxSeq.Value + 1, encodingEpisode.Name, albumId, outputUrl,
                    encodingEpisode.DurationInSecond, encodingEpisode.SubtitleType, encodingEpisode.Subtitle);*/
                var builder = new Episode.Builder();
                builder.Id(id).SequenceNumber(maxSeq.Value + 1).Name(encItem.Name)
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
