public class Article
{
	public long Id { get; set; }//主键
	public string Title { get; set; }//标题
	public string Content { get; set; }//内容
	public List<Comment> Comments { get; set; } = new List<Comment>(); //此文章的若干条评论
}