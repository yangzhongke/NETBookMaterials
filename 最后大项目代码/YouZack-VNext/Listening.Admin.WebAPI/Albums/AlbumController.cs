using Microsoft.AspNetCore.Authorization;

namespace Listening.Admin.WebAPI.Albums;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
[UnitOfWork(typeof(ListeningDbContext))]
public class AlbumController : ControllerBase
{
    private readonly ListeningDbContext dbCtx;
    private readonly ListeningDomainService domainService;
    public AlbumController(ListeningDbContext dbCtx, ListeningDomainService domainService)
    {
        this.dbCtx = dbCtx;
        this.domainService = domainService;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Album>> FindById([RequiredGuid] Guid id)
    {
        var album = await dbCtx.FindAsync<Album>(id);
        return album;
    }

    [HttpGet]
    [Route("{categoryId}")]
    public async Task<ActionResult<IEnumerable<Album>>> FindByCategoryId([RequiredGuid] Guid categoryId)
    {
        var items = dbCtx.Query<Album>().OrderBy(a => a.SequenceNumber).Where(a => a.CategoryId == categoryId);
        return await items.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(AlbumAddRequest req)
    {
        Album album = await domainService.AddAlbumAsync(req.CategoryId,req.Name);
        dbCtx.Add(album);
        return album.Id;
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, AlbumUpdateRequest request)
    {
        var album = await dbCtx.FindAsync<Album>(id);
        if(album==null)
        {
            return NotFound("id没找到");
        }
        album.ChangeName(request.Name);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteById([RequiredGuid] Guid id)
    {
        var album = await dbCtx.FindAsync<Album>(id);
        if (album == null)
        {
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            return NotFound($"没有Id={id}的Album");
        }
        album.SoftDelete();//软删除
        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Hide([RequiredGuid] Guid id)
    {
        var album = await dbCtx.FindAsync<Album>(id);
        if (album == null)
        {
            return NotFound($"没有Id={id}的Album");
        }
        album.Hide();
        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Show([RequiredGuid] Guid id)
    {
        var album = await dbCtx.FindAsync<Album>(id);
        if (album == null)
        {
            return NotFound($"没有Id={id}的Album");
        }
        album.Show();
        return Ok();
    }

    [HttpPut]
    [Route("{categoryId}")]
    public async Task<ActionResult> Sort([RequiredGuid] Guid categoryId, AlbumsSortRequest req)
    {
        await domainService.SortAlbumsAsync(categoryId, req.SortedAlbumIds);
        return Ok();
    }
}