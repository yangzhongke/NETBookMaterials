using Listening.Main.WebAPI.Controllers.Episodes.ViewModels;

namespace Listening.Main.WebAPI.Controllers;
[Route("[controller]/[action]")]
[ApiController]
public class EpisodeController : ControllerBase
{
    private readonly IListeningRepository repository;
    private readonly IMemoryCacheHelper cacheHelper;

    public EpisodeController(IMemoryCacheHelper cacheHelper, IListeningRepository repository)
    {
        this.cacheHelper = cacheHelper;
        this.repository = repository;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<EpisodeVM>> FindById([RequiredGuid] Guid id)
    {
        var episode = await cacheHelper.GetOrCreateAsync($"EpisodeController.FindById.{id}",
            async (e) => EpisodeVM.Create(await repository.GetEpisodeByIdAsync(id), true));
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
            return repository.GetEpisodesByAlbumIdAsync(albumId);
        }
        //加载Episode列表的，默认不加载Subtitle，这样降低流量大小
        var task = cacheHelper.GetOrCreateAsync($"EpisodeController.FindByAlbumId.{albumId}",
            async (e) => EpisodeVM.Create(await FindData(), false));
        return await task;
    }
}