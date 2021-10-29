using System;
using System.IO;
using System.Threading.Tasks;

namespace 异步编程2_22接口抽象类中不能async
{
class Program
{
    static async Task Main(string[] args)
    {
        IMyReader reader = new MyReader();
        string s = await reader.ReadFileAsync(1);
        Console.WriteLine(s);
    }
}

public interface IMyReader
{
    async Task<string> ReadFileAsync(int n);
}

public class MyReader : IMyReader
{
    public async Task<string> ReadFileAsync(int n)
    {
        switch(n)
        {
            case 1:
                return await File.ReadAllTextAsync("d:/1.txt");
            case 2:
                return await File.ReadAllTextAsync("d:/2.txt");
            default:
                return "默认内容";
        }
    }
}
}
