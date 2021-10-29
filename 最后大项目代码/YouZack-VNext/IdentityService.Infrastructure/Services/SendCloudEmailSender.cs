using Identity.Repository;
using IdentityService.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace IdentityService.Infrastructure.Services
{
    public class SendCloudEmailSender : IEmailSender
    {
        private readonly ILogger<SendCloudEmailSender> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptionsSnapshot<SendCloudEmailSettings> sendCloudSettings;
        public SendCloudEmailSender(ILogger<SendCloudEmailSender> logger,
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<SendCloudEmailSettings> sendCloudSettings)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.sendCloudSettings = sendCloudSettings;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            logger.LogInformation("SendCloud Email to {0},subject:{1},body:{2}", toEmail, subject, body);
            var postBody = new Dictionary<string, string>();
            postBody["apiUser"] = sendCloudSettings.Value.ApiUser;
            postBody["apiKey"] = sendCloudSettings.Value.ApiKey;
            postBody["from"] = sendCloudSettings.Value.From;
            postBody["to"] = toEmail;
            postBody["subject"] = subject;
            postBody["html"] = body;

            using (FormUrlEncodedContent httpContent = new FormUrlEncodedContent(postBody))
            {
                var httpClient = httpClientFactory.CreateClient();
                var responseMsg = await httpClient.PostAsync("https://api.sendcloud.net/apiv2/mail/send", httpContent);
                if (!responseMsg.IsSuccessStatusCode)
                {
                    throw new Exception($"发送邮件响应码错误:{responseMsg.StatusCode}");
                }
                var respBody = await responseMsg.Content.ReadAsStringAsync();
                var respModel = respBody.ParseJson<SendCloudResponseModel>();
                if (!respModel.Result)
                {
                    throw new Exception($"发送邮件响应返回失败，状态码：{respModel.StatusCode},消息：{respModel.Message}");
                }
            }
        }
    }
}