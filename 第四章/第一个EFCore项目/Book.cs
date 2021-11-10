public class Book
{
    public long Id { get; set; }//主键
    public string Title { get; set; }//标题
    public DateTime PubTime { get; set; }//发布日期
    public double Price { get; set; }//单价
    public string AuthorName { get; set; }//作者名字
}