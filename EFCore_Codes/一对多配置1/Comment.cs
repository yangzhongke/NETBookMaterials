namespace 一对多配置1
{
    public class Comment
    {
        public long Id { get; set; }
        public Article Article { get; set; }
        public long ArticleId { get; set; }
        public string Message { get; set; }
    }
}
