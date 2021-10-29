
using Listening.Main.WebAPI.Controllers.Albums.ViewModels;

namespace Listening.Main.WebAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly ListeningDbContext dbCtx;
    private readonly IMemoryCacheHelper cacheHelper;
    public AlbumController(ListeningDbContext dbCtx, IMemoryCacheHelper cacheHelper)
    {
        this.dbCtx = dbCtx;
        this.cacheHelper = cacheHelper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<AlbumVM>> FindById([RequiredGuid] Guid id)
    {
        var album = await cacheHelper.GetOrCreateAsync($"AlbumController.FindById.{id}",
           async (e) => AlbumVM.Create(await dbCtx.FindAsync<Album>(id)));
        if (album == null)
        {
            return NotFound();
        }
        return album;
    }

    [HttpGet]
    [Route("{categoryId}")]
    public async Task<ActionResult<AlbumVM[]>> FindByCategoryId([RequiredGuid] Guid categoryId)
    {
        //写到单独的local函数的好处是避免回调中代码太复杂
        Task<Album[]> FindDataAsync()
        {
            return dbCtx.Query<Album>().OrderBy(a => a.SequenceNumber).Where(a => a.CategoryId == categoryId).ToArrayAsync();
        }
        var task = cacheHelper.GetOrCreateAsync($"AlbumController.FindByCategoryId.{categoryId}",
            async (e) => AlbumVM.Create(await FindDataAsync()));
        return await task;
    }
}