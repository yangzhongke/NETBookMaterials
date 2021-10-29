namespace Identity.Repository
{
    public interface ISmsSender
    {
        public Task SendAsync(string phoneNum, params string[] args);
    }
}
