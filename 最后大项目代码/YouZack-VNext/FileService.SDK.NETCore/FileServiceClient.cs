using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Zack.Commons;
using Zack.JWT;

namespace FileService.SDK.NETCore
{
    public class FileServiceClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly Uri serverRoot;
        private readonly JWTOptions optionsSnapshot;
        private readonly ITokenService tokenService;

        public FileServiceClient(IHttpClientFactory httpClientFactory, Uri serverRoot, JWTOptions optionsSnapshot, ITokenService tokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.serverRoot = serverRoot;
            this.optionsSnapshot = optionsSnapshot;
            this.tokenService = tokenService;
        }
        public Task<FileExistsResponse> FileExistsAsync(long fileSize, string sha256Hash, CancellationToken stoppingToken = default)
        {
            string relativeUrl = FormattableStringHelper.BuildUrl($"api/Uploader/FileExists?fileSize={fileSize}&sha256Hash={sha256Hash}");
            Uri requestUri = new Uri(serverRoot, relativeUrl);
            var httpClient = httpClientFactory.CreateClient();
            return httpClient.GetJsonAsync<FileExistsResponse>(requestUri, stoppingToken)!;
        }

        string BuildToken()
        {
            //因为JWT的key等机密信息只有服务器端知道，因此可以这样非常简单的读到配置
            Claim claim = new Claim(ClaimTypes.Role, "Admin");
            return tokenService.BuildToken(new Claim[] { claim }, optionsSnapshot);
        }

        public async Task<Uri> UploadAsync(FileInfo file, CancellationToken stoppingToken = default)
        {
            string token = BuildToken();
            using MultipartFormDataContent content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenRead());
            content.Add(fileContent, "file", file.Name);
            var httpClient = httpClientFactory.CreateClient();
            Uri requestUri = new Uri(serverRoot + "/Uploader/Upload");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var respMsg = await httpClient.PostAsync(requestUri, content, stoppingToken);
            if (!respMsg.IsSuccessStatusCode)
            {
                string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
                throw new HttpRequestException($"上传失败，状态码：{respMsg.StatusCode}，响应报文：{respString}");
            }
            else
            {
                string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
                return respString.ParseJson<Uri>()!;
            }
        }
    }
}
