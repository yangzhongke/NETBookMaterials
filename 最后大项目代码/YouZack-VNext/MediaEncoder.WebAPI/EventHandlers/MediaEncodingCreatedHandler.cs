
using MediaEncoder.Domain.Entities;
using MediaEncoder.Infrastructure;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;

[EventName("MediaEncoding.Created")]
public class MediaEncodingCreatedHandler : DynamicIntegrationEventHandler
{
    private readonly IEventBus eventBus;
    private readonly MEDbContext dbContext;

    public MediaEncodingCreatedHandler(IEventBus eventBus, MEDbContext dbContext)
    {
        this.eventBus = eventBus;
        this.dbContext = dbContext;
    }

    public override async Task HandleDynamic(string eventName, dynamic eventData)
    {
        Guid mediaId = Guid.Parse(eventData.MediaId);
        Uri mediaUrl = new Uri(eventData.MediaUrl);
        string sourceSystem = eventData.SourceSystem;
        string fileName = mediaUrl.Segments.Last();
        string outputFormat = eventData.OutputFormat;
        //保证幂等性，如果这个路径对应的操作已经存在，则直接返回
        bool exists = await dbContext.EncodingItems
            .AnyAsync(e => e.SourceUrl == mediaUrl && e.OutputFormat == outputFormat);
        if (exists)
        {
            return;
        }

        //把任务插入数据库，也可以看作是一种事件，不一定非要放到MQ中才叫事件
        //没有通过领域事件执行，因为如果一下子来很多任务，领域事件就会并发转码，而这种方式则会一个个的转码
        //直接用另一端传来的MediaId作为EncodingItem的主键
        var encodeItem = EncodingItem.Create(mediaId, fileName, mediaUrl, outputFormat, sourceSystem);
        dbContext.Add(encodeItem);
        await dbContext.SaveChangesAsync();
    }
}
