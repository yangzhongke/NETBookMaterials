using FileService.Domain.Entities;
using FileService.Domain.Services;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Zack.ASPNETCore;
using Zack.Commons;

namespace FileService.WebAPI.Uploader;
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
[UnitOfWork(typeof(FSDbContext))]
//todo：要做权限控制，这个接口即对内部系统开放、又对前端用户开放。
public class UploaderController : ControllerBase
{
    private readonly FSDbContext ctx;
    private readonly IStorageClient backupStorage;
    private readonly IStorageClient remoteStorage;
    public UploaderController(FSDbContext ctx, IEnumerable<IStorageClient> storageClients)
    {
        this.ctx = ctx;
        //用这种方式可以解决内置DI不能使用名字注入不同实例的问题，而且从原则上来讲更加优美
        this.backupStorage = storageClients.First(c => c.StorageType == StorageType.Backup);
        this.remoteStorage = storageClients.First(c => c.StorageType == StorageType.Public);
    }

    /// <summary>
    /// 检查是否有和指定的大小和SHA256完全一样的文件
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<FileExistsResponse> FileExists(long fileSize, string sha256Hash)
    {
        var item = await ctx.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize
              && u.FileSHA256Hash == sha256Hash);
        if (item == null)
        {
            return new FileExistsResponse(false, null);
        }
        else
        {
            return new FileExistsResponse(true, item.RemoteUrl);
        }
    }

    //todo: 做好校验，参考OSS的接口，防止被滥用
    //todo：应该由应用服务器向fileserver申请一个上传码（可以指定申请的个数，这个接口只能供应用服务器调用），
    //页面直传只能使用上传码上传一个文件，防止接口被恶意利用。应用服务器要控制发放上传码的频率。
    //todo：再提供一个非页面直传的接口，供服务器用
    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken = default)
    {
        var file = request.File;
        long fileSize = file.Length;
        string fileName = file.FileName;

        using Stream stream = file.OpenReadStream();
        string hash = HashHelper.ComputeSha256Hash(stream);
        stream.Position = 0;//Reset position to the very begining

        DateTime today = DateTime.Today;
        //用日期把文件分散在不同文件夹存储，同时由于加上了文件hash值作为目录，又用用户上传的文件夹做文件名，
        //所以几乎不会发生不同文件冲突的可能
        //用用户上传的文件名保存文件名，这样用户查看、下载文件的时候，文件名更灵活
        string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";

        //查询是否有和上传文件的大小和SHA256一样的文件，如果有的话，就认为是同一个文件
        //虽然说前端可能已经调用FileExists接口检查过了，但是前端可能跳过了，或者有并发上传等问题，所以这里再检查一遍。
        var oldUploadItem = await ctx.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize && u.FileSHA256Hash == hash);
        if (oldUploadItem != null)
        {
            return oldUploadItem.RemoteUrl;
        }
        //backupStorage实现很稳定、速度很快，一般都使用本地存储（文件共享或者NAS）
        Uri backupUrl = await backupStorage.SaveAsync(key, stream, cancellationToken);//保存到本地备份
        stream.Position = 0;
        Uri remoteUrl = await remoteStorage.SaveAsync(key, stream, cancellationToken);//保存到生产的存储系统
        stream.Position = 0;
        Guid id = Guid.NewGuid();
        UploadedItem upItem = UploadedItem.Create(id, fileSize, fileName, hash, backupUrl, remoteUrl);
        ctx.Add(upItem);
        return remoteUrl;
    }
}
