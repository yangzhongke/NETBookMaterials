using System;

namespace LINQ2_24委托2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Func<int, int, string> f1 = delegate (int i1, int i2) {
                return $"{i1}+{i2}={i1 + i2}";
            };
            string s = f1(1, 2);
            Console.WriteLine(s);*/
            /*
            Func<int, int, string> f1 = (i1,i2) =>{
                return $"{i1}+{i2}={i1 + i2}";
            };
            string s = f1(1, 2);
            Console.WriteLine(s);*/

            Func<int, int, string> f1 = (i1, i2) => $"{i1}+{i2}={i1 + i2}";

        }
    }
}
