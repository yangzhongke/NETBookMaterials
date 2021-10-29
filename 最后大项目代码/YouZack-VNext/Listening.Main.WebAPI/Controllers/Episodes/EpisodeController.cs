using Listening.Main.WebAPI.Controllers.Episodes.ViewModels;

namespace Listening.Main.WebAPI.Controllers;
[Route("[controller]/[action]")]
[ApiController]
public class EpisodeController : ControllerBase
{
    private readonly ListeningDbContext dbContext;
    private readonly IMemoryCacheHelper cacheHelper;

    public EpisodeController(ListeningDbContext dbContext, IMemoryCacheHelper cacheHelper)
    {
        this.dbContext = dbContext;
        this.cacheHelper = cacheHelper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<EpisodeVM>> FindById([RequiredGuid] Guid id)
    {
        var episode = await cacheHelper.GetOrCreateAsync($"EpisodeController.FindById.{id}",
            async (e) => EpisodeVM.Create(await this.dbContext.FindAsync<Episode>(id), true));
        if (episode == null)
        {
            return NotFound($"没有Id={id}的Episode");
        }
        return episode;
    }

    [HttpGet]
    [Route("{albumId}")]
    public async Task<ActionResult<EpisodeVM[]>> FindByAlbumId([RequiredGuid] Guid albumId)
    {
        Task<Episode[]> FindData()
        {
            return this.dbContext.Query<Episode>().OrderBy(e => e.SequenceNumber)
            .Where(e => e.AlbumId == albumId).ToArrayAsync();
        }
        //加载Episode列表的，默认不加载Subtitle，这样降低流量大小
        var task = cacheHelper.GetOrCreateAsync($"EpisodeController.FindByAlbumId.{albumId}",
            async (e) => EpisodeVM.Create(await FindData(), false));
        return await task;
    }
}