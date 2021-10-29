using System;
using System.IO;
using System.Threading.Tasks;

namespace 异步编程1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("before write file");
            await File.WriteAllTextAsync("d:/1.txt", "hello async"); 
            Console.WriteLine("before read file");
            string s = await File.ReadAllTextAsync("d:/1.txt");
            Console.WriteLine(s);
        }
        /*
        //不能是async void，必须是Task
        static async Task Main1(string[] args)
        {
            Console.WriteLine("Main1:"+Thread.CurrentThread.ManagedThreadId);
            await A();
            Console.WriteLine("Main2:" + Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
        }

        static async Task<string> A()
        {
            Console.WriteLine("A1:" + Thread.CurrentThread.ManagedThreadId);
            StringBuilder sb = new StringBuilder();
            //for(int i=0;i<10*1000*1000;i++)
            for (int i = 0; i < 1000*10000; i++)
            {
                sb.AppendLine(i.ToString());
            }
            await System.IO.File.WriteAllTextAsync("d:/1.txt", sb.ToString());
            var client = new HttpClient();
            // string s = await client.GetStringAsync("https://www.baidu.com");
            // return s;
            return "";
        }*/
    }
}
