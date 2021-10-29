using System;
using System.Linq;

namespace LINQ2_49统计字母出现次数
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "The precision specifier indicates the desired Number of decimal Places";
            var items = s.Where(c => char.IsLetter(c))//过滤掉非字母
                .Select(c => char.ToLower(c))//把大写字母转换为小写字母
                .GroupBy(c => c)//根据字母进行分组
                .Where(g=>g.Count()>2)//过滤掉出现次数<=2的
                .OrderByDescending(g => g.Count())//按照出现次数排序
                .Select(g=>new { Char=g.Key,Count=g.Count()});
            foreach(var item in items)
            {
                Console.WriteLine($"{item.Char}出现{item.Count}次");
            }
        }
    }
}