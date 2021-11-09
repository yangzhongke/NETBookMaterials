using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace 异步编程2_10_不想切换线程1
{
    /*
    class Program
    {
        static async Task Main(string[] args)
        {
            string s1 = await ReadFileAsync(1);
            System.Console.WriteLine(s1);
        }

        static async Task<string> ReadFileAsync(int num)
        {
            switch(num)
            {
                case 1:
                    return await File.ReadAllTextAsync("d:/1.txt");
                case 2:
                    return await File.ReadAllTextAsync("d:/2.txt");
                default:
                    throw new ArgumentException("num invalid");
            }
        }
    }*/

    class Program
    {
        static async Task Main(string[] args)
        {
            string s1 = await ReadFileAsync(1);
            System.Console.WriteLine(s1);

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
                    throw new ArgumentException("num invalid");
            }
        }
    }
}