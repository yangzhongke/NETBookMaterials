
using MediaEncoder.Domain.Entities;
using MediaEncoder.Infrastructure;
using MediaEncoder.WebAPI.Controllers;

namespace MediaEncoder.WebAPI.Services;
public class EncoderService
{
    private readonly MEDbContext dbContext;

    public EncoderService(MEDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Start(StartRequest request, CancellationToken cancellationToken = default)
    {
        string srcFileName = request.SourceFileName;
        Uri sourceUrl = request.SourceUrl;
        string outputFormat = request.OutputFormat;
        string sourceSystem = request.SourceSystem;

        /*
        //保证幂等性，如果这个路径对应的操作已经存在，则直接返回
        var prevInstance = await dbContext.EncodingItems
            .FirstOrDefaultAsync(e => e.SourceUrl == sourceUrl && e.OutputFormat == outputFormat);
        if (prevInstance != null)
        {
            return;
        }*/

        //把任务插入数据库，也可以看作是一种事件，不一定非要放到MQ中才叫事件
        //没有通过领域事件执行，因为如果一下子来很多任务，领域事件就会并发转码，而这种方式则会一个个的转码
        Guid id = request.MediaId;//直接用另一端传来的MediaId作为EncodingItem的主键
        var encodeItem = EncodingItem.Create(id, srcFileName, sourceUrl, outputFormat, sourceSystem);
        dbContext.Add(encodeItem);
        await dbContext.SaveChangesAsync();
    }
}
