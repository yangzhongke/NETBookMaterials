using FileService.SDK.NETCore;
using MediaEncoder.Domain;
using MediaEncoder.Domain.Entities;
using MediaEncoder.Infrastructure;
using MediaEncoder.WebAPI.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System.Net;
using System.Net.Http;
using Zack.Commons;
using Zack.EventBus;
using Zack.JWT;

namespace MediaEncoder.WebAPI.BgServices;
public class EncodingBgService : BackgroundService
{
    private readonly MEDbContext dbContext;
    private readonly IMediaEncoderRepository repository;
    private readonly List<RedLockMultiplexer> redLockMultiplexerList;
    private readonly ILogger<EncodingBgService> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly MediaEncoderFactory encoderFactory;
    private readonly IOptionsSnapshot<FileServiceOptions> optionFileService;
    private readonly IServiceScope serviceScope;
    private readonly IEventBus eventBus;
    private readonly IOptionsSnapshot<JWTOptions> optionJWT;
    private readonly ITokenService tokenService;

    public EncodingBgService(IServiceScopeFactory spf)
    {
        //MEDbContext等是Scoped，而BackgroundService是Singleton，所以不能直接注入，需要手动开启一个新的Scope
        this.serviceScope = spf.CreateScope();
        var sp = serviceScope.ServiceProvider;
        this.dbContext = sp.GetRequiredService<MEDbContext>(); ;
        //生产环境中，RedLock需要五台服务器才能体现价值，测试环境无所谓
        IConnectionMultiplexer connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
        this.redLockMultiplexerList = new List<RedLockMultiplexer> { new RedLockMultiplexer(connectionMultiplexer) };
        this.logger = sp.GetRequiredService<ILogger<EncodingBgService>>();
        this.httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
        this.encoderFactory = sp.GetRequiredService<MediaEncoderFactory>();
        this.optionFileService = sp.GetRequiredService<IOptionsSnapshot<FileServiceOptions>>();
        this.eventBus = sp.GetRequiredService<IEventBus>();
        this.optionJWT = sp.GetRequiredService<IOptionsSnapshot<JWTOptions>>();
        this.tokenService = sp.GetRequiredService<ITokenService>();
        this.repository = sp.GetRequiredService<IMediaEncoderRepository>();
    }

    private async Task ProcessItemAsync(EncodingItem readyItem, CancellationToken stoppingToken)
    {
        Guid id = readyItem.Id;
        var expiry = TimeSpan.FromSeconds(30);
        var redlockFactory = RedLockFactory.Create(redLockMultiplexerList);
        string lockKey = $"MediaEncoder.EncodingItem.{id}";
        //用RedLock分布式锁，锁定对EncodingItem的访问
        using (var redLock = await redlockFactory.CreateLockAsync(lockKey, expiry))
        {
            if (!redLock.IsAcquired)
            {
                logger.LogInterpolatedWarning($"获取{lockKey}锁失败，已被抢走");
                //获得锁失败，锁已经被别人抢走了，说明这个任务被别的实例处理了（有可能有服务器集群来分担转码压力）
                return;//再去抢下一个
            }
            readyItem.Start();
            await dbContext.SaveChangesAsync(stoppingToken);//立即保存一下状态的修改
            //发出一次集成事件
            //开始下载源文件
            string tempDir = Path.Combine(Path.GetTempPath(), "MediaEncodingDir");
            //源文件的临时保存路径
            string sourceFullPath = Path.Combine(tempDir, Guid.NewGuid() + "." + Path.GetExtension(readyItem.Name));

            new FileInfo(sourceFullPath).Directory!.Create();//创建可能不存在的文件夹
            logger.LogInterpolatedInformation($"开始执行Id={id}的任务，准备从{readyItem.SourceUrl}下载到{sourceFullPath}");
            HttpClient httpClient = httpClientFactory.CreateClient();
            var statusCode = await httpClient.DownloadFileAsync(readyItem.SourceUrl, sourceFullPath, stoppingToken);
            if (statusCode != HttpStatusCode.OK)
            {
                logger.LogInterpolatedWarning($"下载Id={readyItem.Id}，Url={readyItem.SourceUrl}的文件失败，状态码{statusCode}");
                readyItem.Fail($"下载失败,状态码{statusCode}");

                File.Delete(sourceFullPath);
                return;
            }
            logger.LogInterpolatedInformation($"下载Id={id}成功，开始计算Hash值");

            long fileSize = new FileInfo(sourceFullPath).Length;
            string sourceFileHash;
            //这里不能简写成using FileStream streamSrc = File.OpenRead(sourceFullPath);这样的形式，因为这是出了方法范围后才会Dispose
            //这样File.Delete(sourceFullPath)就删除失败了
            using (FileStream streamSrc = File.OpenRead(sourceFullPath))
            {
                sourceFileHash = HashHelper.ComputeSha256Hash(streamSrc);
            }
            //如果之前存在过和这个文件大小、hash一样的文件，就认为重复了
            var prevInstance = await repository.FindCompletedOneAsync(sourceFileHash,fileSize);
            if (prevInstance != null)
            {
                logger.LogInterpolatedInformation($"检查Id={id}Hash值成功，发现已经存在相同大小和Hash值的旧任务Id={prevInstance.Id}，返回！");
                eventBus.Publish("MediaEncoding.Duplicated", new { readyItem.Id, readyItem.SourceSystem, OutputUrl = prevInstance.OutputUrl });
                readyItem.Complete(prevInstance.OutputUrl!);
                File.Delete(sourceFullPath);
                return;
            }

            //开始转码
            string outputFormat = readyItem.OutputFormat;
            string destFullPath = Path.Combine(tempDir, Guid.NewGuid() + "." + outputFormat);
            logger.LogInterpolatedInformation($"Id={id}开始转码，源路径:{sourceFullPath},目标路径:{destFullPath}，目标格式:{outputFormat}");
            var encoder = encoderFactory.Create(outputFormat);
            try
            {
                await encoder.EncodeAsync(sourceFullPath, destFullPath, outputFormat, null, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogInterpolatedError($"Id={id}转码失败", ex);
                readyItem.Fail($"转码失败,{ex}");
                File.Delete(sourceFullPath);
                File.Delete(destFullPath);
                return;
            }

            //开始上传
            logger.LogInterpolatedInformation($"Id={id}转码成功，开始准备上传");
            Uri urlRoot = optionFileService.Value.UrlRoot;
            FileServiceClient fileService = new FileServiceClient(httpClientFactory, urlRoot,
                optionJWT.Value, tokenService);
            Uri uploadedUrl = await fileService.UploadAsync(destFullPath, stoppingToken);
            File.Delete(sourceFullPath);
            File.Delete(destFullPath);
            readyItem.Complete(uploadedUrl);
            readyItem.ChangeFileMeta(fileSize, sourceFileHash);

            logger.LogInterpolatedInformation($"Id={id}转码结果上传成功");
            //发出集成事件和领域事件
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //获取所有处于Ready状态的任务
            //ToListAsync()可以避免在循环中再用DbContext去查询数据导致的“There is already an open DataReader associated with this Connection which must be closed first.”
            var readyItems = await repository.FindAsync(ItemStatus.Ready);
            foreach (EncodingItem readyItem in readyItems)
            {
                try
                {
                    await ProcessItemAsync(readyItem, stoppingToken);//因为转码比较消耗cpu等资源，因此串行转码
                }
                catch (Exception ex)
                {
                    readyItem.Fail(ex);
                }
                await this.dbContext.SaveChangesAsync(stoppingToken);
            }
            await Task.Delay(5000);//暂停5s，避免没有任务的时候CPU空转
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        this.serviceScope.Dispose();
    }
}
