using System;
using System.Collections.Generic;
using System.Text;

namespace DI3_51容器基本使用1
{
    public interface ITestService
    {
        public string Name { get; set; }
        public void SayHi();
    }
}
