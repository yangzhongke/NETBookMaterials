using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace 异步编程2_4不要忘了await
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string fileName = "d:/1.txt";
            File.Delete(fileName);
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<5000000;i++)
            {
                sb.AppendLine("hello async");
            }
            File.WriteAllTextAsync(fileName, sb.ToString());
            string s = await File.ReadAllTextAsync(fileName);
            Console.WriteLine(s);
        }
    }
}
