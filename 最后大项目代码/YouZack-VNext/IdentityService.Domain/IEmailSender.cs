namespace Identity.Repository
{
    public interface IEmailSender
    {
        public Task SendAsync(string toEmail, string subject, string body);
    }
}
