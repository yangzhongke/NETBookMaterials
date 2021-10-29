using System;
using System.Collections.Generic;
using System.Text;

namespace DI3_51容器基本使用1
{
    public class TestServiceImpl : ITestService
    {
        public string Name { get; set; }

        public void SayHi()
        {
            Console.WriteLine($"Hi, I'm {Name}");
        }
    }
}
