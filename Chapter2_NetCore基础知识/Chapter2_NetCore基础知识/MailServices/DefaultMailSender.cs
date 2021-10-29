using ConfigServices;
using LogServices;
using System;

namespace MailServices
{
    class DefaultMailSender : IMailSender
    {
        private readonly IConfigReader config;
        private readonly ILogProvider log;

        public DefaultMailSender(IConfigReader config, ILogProvider log)
        {
            this.config = config;
            this.log = log;
        }

        public void Send(string toAddress, string title, string body)
        {
            log.LogInfo("开始准备发送邮件");
            try
            {
                string smtpServer = this.config.GetValue("SmtpServer");
                string senderEmail = this.config.GetValue("SenderEmail");
                string password = this.config.GetValue("SmtpPassword");
                Console.WriteLine($"模拟向{toAddress}发送邮件成功，smtpServer={smtpServer},senderEmail={senderEmail},password={senderEmail},标题:{title},内容:{body}");
                log.LogInfo("完成发送邮件");
            }
            catch(Exception ex)
            {
                log.LogError("邮件发送失败：" + ex);
                throw;
            }  
        }
    }
}