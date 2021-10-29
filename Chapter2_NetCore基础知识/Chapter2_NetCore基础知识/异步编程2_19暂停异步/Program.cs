using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace 异步编程2_19暂停异步
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using(HttpClient httpClient = new HttpClient())
            {
                string s1 = await httpClient.GetStringAsync("https://www.baidu.com");
                await Task.Delay(3000);
                string s2 = await httpClient.GetStringAsync("https://www.taobao.com");
            }
        }
    }
}
