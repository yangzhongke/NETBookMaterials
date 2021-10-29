using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace SystemServices
{
    class ReadFileService
    {
        private readonly ILogger logger;

        public ReadFileService(ILogger<ReadFileService> logger)
        {
            this.logger = logger;
        }
       public Task<string> Read(string path)
        {
            logger.LogInformation("准备读文件{0}", path);
            if (!File.Exists(path))
            {
                logger.LogWarning("要读取的文件{0}不存在", path);
                return Task.FromResult<string>(null);
            }
            logger.LogInformation("文件{0}尺寸：{1}", path, new FileInfo(path).Length);
            return File.ReadAllTextAsync(path);
        }
    }
}