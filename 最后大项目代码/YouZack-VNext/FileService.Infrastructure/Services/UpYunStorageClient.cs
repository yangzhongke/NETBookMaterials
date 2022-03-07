using FileService.Domain;
using Microsoft.Extensions.Options;
using System.Net.Http;
using UpYun.NETCore;
using Zack.Commons;

namespace FileService.Infrastructure.Services
{
    /// <summary>
    /// 又拍云存储服务
    /// </summary>
    class UpYunStorageClient : IStorageClient
    {
        private IOptionsSnapshot<UpYunStorageOptions> options;
        private IHttpClientFactory httpClientFactory;
        public UpYunStorageClient(IOptionsSnapshot<UpYunStorageOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            this.options = options;
            this.httpClientFactory = httpClientFactory;
        }

        public StorageType StorageType => StorageType.Public;

        static string ConcatUrl(params string[] segments)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                string s = segments[i];
                if (s.Contains(".."))
                {
                    throw new ArgumentException("路径中不能含有..");
                }
                segments[i] = s.Trim('/');//把开头结尾的/去掉
            }
            return string.Join('/', segments);
        }

        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
        {
            if (key.StartsWith('/'))
            {
                throw new ArgumentException("key should not start with /", nameof(key));
            }
            byte[] bytes = content.ToArray();
            if (bytes.Length <= 0)
            {
                throw new ArgumentException("file cannot be empty", nameof(content));
            }
            string bucketName = options.Value.BucketName;
            string userName = options.Value.UserName;
            string password = options.Value.Password;
            string pathRoot = options.Value.WorkingDir;

            string url = ConcatUrl(options.Value.UrlRoot, pathRoot, key);//web访问的文件网址
            string fullPath = "/" + ConcatUrl(pathRoot, key);//又拍云的上传路径
            UpYunClient upyun = new UpYunClient(bucketName, userName, password, httpClientFactory);
            var upyunResult = await upyun.WriteFileAsync(fullPath, bytes, true, cancellationToken);
            if (upyunResult.IsOK == false)
            {
                throw new HttpRequestException("uploading to upyun failed:" + upyunResult.Msg);
            }
            return new Uri(url);
        }
    }
}
