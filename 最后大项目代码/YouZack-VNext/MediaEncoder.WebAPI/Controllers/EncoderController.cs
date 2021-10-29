using MediaEncoder.Domain.Entities;
using MediaEncoder.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

namespace MediaEncoder.WebAPI.Controllers;

//把Web API看成应用程序层而不是UI层会更简单，比较折中的做法。前端或者App是UI层。
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class EncoderController : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly MEDbContext dbContext;

    public EncoderController(IHttpClientFactory httpClientFactory, MEDbContext dbContext)
    {
        this.httpClientFactory = httpClientFactory;
        this.dbContext = dbContext;
    }

    [HttpPost]
    public async Task Start(StartRequest request, CancellationToken cancellationToken = default)
    {
        string srcFileName = request.SourceFileName;
        Uri sourceUrl = request.SourceUrl;
        string outputFormat = request.OutputFormat;
        string sourceSystem = request.SourceSystem;

        //保证幂等性，如果这个路径对应的操作已经存在，则直接返回
        var prevInstance = await dbContext.Query<EncodingItem>()
            .FirstOrDefaultAsync(e => e.SourceUrl == sourceUrl && e.OutputFormat == outputFormat);
        if (prevInstance != null)
        {
            return;
        }

        //把任务插入数据库，也可以看作是一种事件，不一定非要放到MQ中才叫事件
        //没有通过领域事件执行，因为如果一下子来很多任务，领域事件就会并发转码，而这种方式则会一个个的转码
        Guid id = request.MediaId;//直接用另一端传来的MediaId作为EncodingItem的主键
        var encodeItem = EncodingItem.Create(id, srcFileName, sourceUrl, outputFormat, sourceSystem);
        dbContext.Add(encodeItem);
        await dbContext.SaveChangesAsync();
    }
}
