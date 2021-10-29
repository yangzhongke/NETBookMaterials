namespace MailServices
{
    public interface IMailSender
    {
        /// <summary>
        /// 给toAddress发送邮件
        /// </summary>
        /// <param name="toAddress">收件人</param>
        /// <param name="title">标题</param>
        /// <param name="body">内容</param>
        public void Send(string toAddress, string title, string body);
    }
}