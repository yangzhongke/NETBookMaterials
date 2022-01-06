using System;
using System.IO;

namespace NETStandard类库
{
    public class DemoNetStandardClass
    {
        public static void Test()
        {
            Console.WriteLine(typeof(FileStream).Assembly.Location);
        }
    }
}
