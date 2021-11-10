using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("T_Books")]
public class Book
{
    public long Id { get; set; }//主键
    [MaxLength(50)]
    [Required]
    public string Title { get; set; }//标题
    public DateTime PubTime { get; set; }//发布日期
    public double Price { get; set; }//单价
    [MaxLength(20)]
    [Required]
    public string AuthorName { get; set; }//作者名字
}