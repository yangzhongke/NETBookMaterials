using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 异步编程2_9线程切换1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("1-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5000000; i++)
            //for (int i = 0; i < 1; i++)
            {
                sb.AppendLine("Hello async");
            }
            await File.WriteAllTextAsync("d:/1.txt", sb.ToString());
            Console.WriteLine("2-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
            await File.WriteAllTextAsync("d:/2.txt", sb.ToString());
            Console.WriteLine("3-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
            File.WriteAllText("d:/3.txt", sb.ToString());//同步写入
            Console.WriteLine("4-ThreadId=" + Thread.CurrentThread.ManagedThreadId);
        }
    }
}