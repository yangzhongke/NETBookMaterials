using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace 异步编程2_5封装下载Http
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("开始下载baidu.com");
            int i1 = await DownloadAsync("https://www.baidu.com", "d:/baidu.html");
            Console.WriteLine($"baidu.com 下载完成，长度{i1}");
            Console.WriteLine("开始下载taobao.com");
            int i2 = await DownloadAsync("https://www.taobao.com", "d:/taobao.html");
            Console.WriteLine($"taobao.com 下载完成，长度{i2}");
            int i3 = await DownloadAsync("https://www.qq.com", "d:/qq.html");
            Console.WriteLine($"qq.com 下载完成，长度{i3}");
        }

        /// <summary>
        /// 把网址url的内容下载到文件destFilePath中
        /// </summary>
        /// <param name="url">要下载的网址</param>
        /// <param name="destFilePath">要保存的文件路径</param>
        /// <returns></returns>
        static async Task<int> DownloadAsync(string url,string destFilePath)
        {
            string body;
            using(HttpClient httpClient = new HttpClient())
            {
                body = await httpClient.GetStringAsync(url);
            }
            await File.WriteAllTextAsync(destFilePath, body);
            return body.Length;
        }
    }
}