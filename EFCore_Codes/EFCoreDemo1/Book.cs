using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreDemo1
{
    public class Book
    {
        public long Id { get; set; }//主键
        public string Title { get; set; }//标题
        public DateTime PubTime { get; set; }//发布日期
        public double Price { get; set; }//单价
        public string AuthorName { get; set; }//作者名字
        public bool IsDeleted { get; set; } = false;

        public override string ToString()
        {
            return $"Id={Id},Title={Title},PubTime={PubTime},Price={Price},AuthorName={AuthorName}";
        }
    }
}
