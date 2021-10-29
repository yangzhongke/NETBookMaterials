using System;
using System.IO;
using System.Threading.Tasks;
namespace 异步编程2_3不要忘了await
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string fileName = "d:/1.txt";
            File.Delete(fileName);
            File.WriteAllTextAsync(fileName, "hello async");
            string s = await File.ReadAllTextAsync(fileName);
            Console.WriteLine(s);
        }
    }
}



