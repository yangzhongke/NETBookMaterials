
using MediaEncoder.WebAPI.Controllers;
using MediaEncoder.WebAPI.Services;
using Zack.EventBus;

namespace MediaEncoder.WebAPI.EventHandlers;

[EventName("MediaEncoding.Created")]
public class MediaEncodingCreatedHandler : DynamicIntegrationEventHandler
{
    private readonly IEventBus eventBus;
    private readonly EncoderService encoderService;

    public MediaEncodingCreatedHandler(IEventBus eventBus, EncoderService encoderService)
    {
        this.eventBus = eventBus;
        this.encoderService = encoderService;
    }

    public override Task HandleDynamic(string eventName, dynamic eventData)
    {
        Guid mediaId = Guid.Parse(eventData.MediaId);
        Uri mediaUrl = new Uri(eventData.MediaUrl);
        string sourceSystem = eventData.SourceSystem;
        string fileName = mediaUrl.Segments.Last();
        string outputFormat = eventData.OutputFormat;
        return encoderService.Start(new StartRequest(mediaId, fileName, mediaUrl, outputFormat, sourceSystem));
    }
}
