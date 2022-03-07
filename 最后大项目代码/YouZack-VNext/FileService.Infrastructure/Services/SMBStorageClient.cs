using FileService.Domain;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Services
{
    /// <summary>
    /// 用局域网内共享文件夹或者本机磁盘当备份服务器的实现类
    /// </summary>
    class SMBStorageClient : IStorageClient
    {
        private IOptionsSnapshot<SMBStorageOptions> options;
        public SMBStorageClient(IOptionsSnapshot<SMBStorageOptions> options)
        {
            this.options = options;
        }

        public StorageType StorageType => StorageType.Backup;

        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
        {
            if (key.StartsWith('/'))
            {
                throw new ArgumentException("key should not start with /", nameof(key));
            }
            string workingDir = options.Value.WorkingDir;
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);//get the directory
            if (!Directory.Exists(fullDir))//automatically create dir
            {
                Directory.CreateDirectory(fullDir);
            }
            if (File.Exists(fullPath))//如果已经存在，则尝试删除
            {
                File.Delete(fullPath);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await content.CopyToAsync(outStream, cancellationToken);
            return new Uri(fullPath);
        }
    }
}
