using System;
using System.Collections.Generic;
using System.Text;

namespace DI3_51容器基本使用1
{
public class TestServiceImpl2 : ITestService
{
    public string Name { get; set; }

    public void SayHi()
    {
        Console.WriteLine($"你好，我是{Name}");
    }
}
}
