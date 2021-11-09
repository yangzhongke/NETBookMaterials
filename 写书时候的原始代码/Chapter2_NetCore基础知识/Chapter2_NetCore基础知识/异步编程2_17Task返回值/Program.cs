using System;
using System.IO;
using System.Threading.Tasks;

namespace 异步编程2_17Task返回值
{
    class Program
    {
        static async Task Main(string[] args)
        {
string s1 = File.ReadAllTextAsync("d:/1.txt").Result;
string s2= File.ReadAllTextAsync("d:/1.txt").GetAwaiter().GetResult();
File.WriteAllTextAsync("d:/1.txt", "hello").Wait();
            await WriteFileAsync(3, "hello");
            string s1 = await ReadFileAsync(5);
            Console.WriteLine(s1);
            
        }

        static Task WriteFileAsync(int num,string content)
        {
            switch (num)
            {
                case 1:
                    return File.WriteAllTextAsync("d:/1.txt",content);
                case 2:
                    return File.WriteAllTextAsync("d:/2.txt", content);
                default:
                    Console.WriteLine("文件暂时不可用");
                    return Task.CompletedTask;
            }
        }

        static Task<string> ReadFileAsync(int num)
        {
            switch (num)
            {
                case 1:
                    return File.ReadAllTextAsync("d:/1.txt");
                case 2:
                    return File.ReadAllTextAsync("d:/2.txt");
                default:
                    return Task.FromResult("Love");
            }
        }
    }
}