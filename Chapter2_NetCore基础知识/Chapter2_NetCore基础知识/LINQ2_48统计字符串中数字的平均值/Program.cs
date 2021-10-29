using System;
using System.Linq;

namespace LINQ2_48统计字符串中数字的平均值
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "61,90,100,99,18,22,38,66,80,93,55,50,89";
            double avg = s.Split(',').Select(e => Convert.ToInt32(e)).Average();
            Console.WriteLine(avg);
        }
    }
}