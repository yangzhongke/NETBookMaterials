
using Microsoft.AspNetCore.Authorization;

namespace Listening.Admin.WebAPI.Albums;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AlbumController : ControllerBase
{
    private readonly ListeningDbContext dbCtx;
    public AlbumController(ListeningDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
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
        //获取本Category中的最大序号
        int? maxSeq = await dbCtx.Query<Album>().Where(c => c.CategoryId == req.CategoryId)
            .MaxAsync(c => (int?)c.SequenceNumber);
        maxSeq = maxSeq ?? 0;
        var id = Guid.NewGuid();
        Album album = Album.Create(id, maxSeq.Value + 1, req.Name, req.CategoryId);
        dbCtx.Add(album);
        await dbCtx.SaveChangesAsync();
        return id;
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, AlbumUpdateRequest request)
    {
        var album = await dbCtx.FindAsync<Album>(id);
        album.ChangeName(request.Name);
        await dbCtx.SaveChangesAsync();
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
        await dbCtx.SaveChangesAsync();
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
        await dbCtx.SaveChangesAsync();
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
        await dbCtx.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    [Route("{categoryId}")]
    public async Task<ActionResult> Sort([RequiredGuid] Guid categoryId, AlbumsSortRequest req)
    {
        Guid[] idsInDB = await dbCtx.Query<Album>().Where(a => a.CategoryId == categoryId)
            .Select(a => a.Id).ToArrayAsync();
        if (!idsInDB.SequenceIgnoredEqual(req.SortedAlbumIds))
        {
            return this.APIError(1, $"提交的待排序Id中必须是categoryId={categoryId}分类下所有的Id");
        }

        int seqNum = 1;
        //一个in语句一次性取出来更快，不过在非性能关键节点，业务语言比性能更重要
        foreach (Guid albumId in req.SortedAlbumIds)
        {
            var album = await dbCtx.FindAsync<Album>(albumId);
            if (album == null)
            {
                return this.APIError(2, $"albumId={albumId}不存在");
            }
            album.ChangeSequenceNumber(seqNum);//顺序改序号
            seqNum++;
        }
        await dbCtx.SaveChangesAsync();
        return Ok();
    }
}