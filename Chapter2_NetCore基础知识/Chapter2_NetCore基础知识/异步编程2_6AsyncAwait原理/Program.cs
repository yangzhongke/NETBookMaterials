using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace 异步编程2_6AsyncAwait原理
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string html = await httpClient.GetStringAsync("https://www.taobao.com");
                Console.WriteLine(html);
            }
            string destFilePath = "d:/1.txt";
            string content = "hello async and await";
            await File.WriteAllTextAsync(destFilePath, content);
            string content2 = await File.ReadAllTextAsync(destFilePath);
            Console.WriteLine(content2);
        }
    }
}