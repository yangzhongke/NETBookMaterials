using Microsoft.AspNetCore.Authorization;
using Zack.EventBus;

namespace Listening.Admin.WebAPI.Episodes;
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
[UnitOfWork(typeof(ListeningDbContext))]
public class EpisodeController : ControllerBase
{
    private IListeningRepository repository;
    private readonly ListeningDbContext dbContext;
    private readonly EncodingEpisodeHelper encodingEpisodeHelper;
    private readonly IEventBus eventBus;
    private readonly ListeningDomainService domainService;
    public EpisodeController(ListeningDbContext dbContext,
            EncodingEpisodeHelper encodingEpisodeHelper,
            IEventBus eventBus, ListeningDomainService domainService, IListeningRepository repository)
    {
        this.dbContext = dbContext;
        this.encodingEpisodeHelper = encodingEpisodeHelper;
        this.eventBus = eventBus;
        this.domainService = domainService;
        this.repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(EpisodeAddRequest req)
    {
        //如果上传的是m4a，不用转码，直接存到数据库
        if (req.AudioUrl.ToString().EndsWith("m4a", StringComparison.OrdinalIgnoreCase))
        {
            Episode episode = await domainService.AddEpisodeAsync(req.Name, req.AlbumId,
                req.AudioUrl, req.DurationInSecond, req.SubtitleType, req.Subtitle);
            dbContext.Add(episode);
            return episode.Id;
        }
        else
        {
            //非m4a文件需要先转码，为了避免非法数据污染业务数据，增加业务逻辑麻烦，按照DDD的原则，不完整的Episode不能插入数据库
            //先临时插入Redis，转码完成再插入数据库
            Guid episodeId = Guid.NewGuid();
            EncodingEpisodeInfo encodingEpisode = new EncodingEpisodeInfo(episodeId, req.Name, req.AlbumId, req.DurationInSecond, req.Subtitle, req.SubtitleType, "Created");
            await encodingEpisodeHelper.AddEncodingEpisodeAsync(episodeId, encodingEpisode);

            //通知转码
            eventBus.Publish("MediaEncoding.Created", new { MediaId = episodeId, MediaUrl = req.AudioUrl, OutputFormat = "m4a", SourceSystem = "Listening" });//启动转码
            return episodeId;
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, EpisodeUpdateRequest request)
    {
        var episode = await repository.GetEpisodeByIdAsync(id);
        if (episode == null)
        {
            return NotFound("id没找到");
        }
        episode.ChangeName(request.Name);
        episode.ChangeSubtitle(request.SubtitleType, request.Subtitle);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteById([RequiredGuid] Guid id)
    {
        var album = await repository.GetEpisodeByIdAsync(id);
        if (album == null)
        {
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            return NotFound($"没有Id={id}的Episode");
        }
        album.SoftDelete();//软删除
        return Ok();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Episode>> FindById([RequiredGuid] Guid id)
    {
        //因为这是后台系统，所以不在乎把 Episode全部内容返回给客户端的问题，以后如果开放给外部系统再定义ViewModel
        var episode = await repository.GetEpisodeByIdAsync(id);
        if (episode == null)
        {
            return NotFound($"没有Id={id}的Episode");
        }
        return episode;
    }

    [HttpGet]
    [Route("{albumId}")]
    public Task<Episode[]> FindByAlbumId([RequiredGuid] Guid albumId)
    {
        return repository.GetEpisodesByAlbumIdAsync(albumId);
    }

    //获取albumId下所有的转码任务
    [HttpGet]
    [Route("{albumId}")]
    public async Task<ActionResult<EncodingEpisodeInfo[]>> FindEncodingEpisodesByAlbumId([RequiredGuid] Guid albumId)
    {
        List<EncodingEpisodeInfo> list = new List<EncodingEpisodeInfo>();
        var episodeIds = await encodingEpisodeHelper.GetEncodingEpisodeIdsAsync(albumId);
        foreach (Guid episodeId in episodeIds)
        {
            var encodingEpisode = await encodingEpisodeHelper.GetEncodingEpisodeAsync(episodeId);
            if (!encodingEpisode.Status.EqualsIgnoreCase("Completed"))//不显示已经完成的
            {
                list.Add(encodingEpisode);
            }
        }
        return list.ToArray();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Hide([RequiredGuid] Guid id)
    {
        var episode = await repository.GetEpisodeByIdAsync(id);
        if (episode == null)
        {
            return NotFound($"没有Id={id}的Category");
        }
        episode.Hide();
        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Show([RequiredGuid] Guid id)
    {
        var episode = await repository.GetEpisodeByIdAsync(id);
        if (episode == null)
        {
            return NotFound($"没有Id={id}的Category");
        }
        episode.Show();
        return Ok();
    }

    [HttpPut]
    [Route("{albumId}")]
    public async Task<ActionResult> Sort([RequiredGuid] Guid albumId, EpisodesSortRequest req)
    {
        await domainService.SortEpisodesAsync(albumId, req.SortedEpisodeIds);
        return Ok();
    }
}