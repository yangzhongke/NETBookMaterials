using Microsoft.AspNetCore.Authorization;

namespace Listening.Admin.WebAPI.Categories;
[Route("[controller]/[action]")]
[Authorize(Roles = "Admin")]
[ApiController]
//供后台用的增删改查接口不用缓存
public class CategoryController : ControllerBase
{
    private readonly ListeningDbContext dbContext;
    public CategoryController(ListeningDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<Category[]> FindAll()
    {
        return dbContext.Query<Category>().OrderBy(c => c.SequenceNumber).ToArray();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Category?>> FindById([RequiredGuid] Guid id)
    {
        //返回ValueTask的需要await的一下
        var cat = await dbContext.FindAsync<Category>(id);
        if (cat == null)
        {
            return NotFound($"没有Id={id}的Category");
        }
        else
        {
            return cat;
        }
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(CategoryAddRequest req)
    {
        //获取最大序号，把序号+1作为新增加的序号
        //后台操作，不会有并发的问题
        //MaxAsync(c => (int?)c.SequenceNumber) 这样可以处理一条数据都没有的问题
        int? maxSeq = await dbContext.Query<Category>().MaxAsync(c => (int?)c.SequenceNumber);
        maxSeq = maxSeq ?? 0;
        var id = Guid.NewGuid();
        Category category = Category.Create(id, maxSeq.Value + 1, req.Name, req.CoverUrl);
        dbContext.Add(category);
        await dbContext.SaveChangesAsync();
        return id;
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, CategoryUpdateRequest request)
    {
        var cat = await dbContext.FindAsync<Category>(id);
        cat.ChangeName(request.Name);
        cat.ChangeCoverUrl(request.CoverUrl);
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteById([RequiredGuid] Guid id)
    {
        var cat = await dbContext.FindAsync<Category>(id);
        if (cat == null)
        {
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            return NotFound($"没有Id={id}的Category");
        }
        cat.SoftDelete();//软删除
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Sort(CategoriesSortRequest req)
    {
        Guid[] idsInDB = await dbContext.Query<Category>().Select(a => a.Id).ToArrayAsync();
        if (!idsInDB.SequenceIgnoredEqual(req.SortedCategoryIds))
        {
            return this.APIError(1, $"提交的待排序Id中必须是所有的分类Id");
        }
        int seqNum = 1;
        //一个in语句一次性取出来更快，不过在非性能关键节点，业务语言比性能更重要
        foreach (Guid catId in req.SortedCategoryIds)
        {
            var cat = await dbContext.FindAsync<Category>(catId);
            if (cat == null)
            {
                return NotFound($"categoryId={catId}不存在");
            }
            cat.ChangeSequenceNumber(seqNum);//顺序改序号
            seqNum++;
        }
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
