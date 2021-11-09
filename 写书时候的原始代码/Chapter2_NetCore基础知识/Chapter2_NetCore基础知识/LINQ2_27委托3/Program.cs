using System;
using System.IO;

namespace LINQ2_27委托3
{
    class Program
    {
        static void Main(string[] args)
        {
Action<int, string> a1 = (age, name) => Console.WriteLine($"年龄{age},姓名{name}");
a1(18, "yzk");
        }

        
    }
}
