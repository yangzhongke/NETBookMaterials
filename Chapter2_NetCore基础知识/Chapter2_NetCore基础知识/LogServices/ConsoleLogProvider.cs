using System;

namespace LogServices
{
    public class ConsoleLogProvider : ILogProvider
    {
        public void LogError(string msg)
        {
            //修改错误信息的文字颜色和背景色
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(DateTime.Now+" Error:"+msg);
            Console.ResetColor();//恢复默认颜色
        }

        public void LogInfo(string msg)
        {
            Console.WriteLine(DateTime.Now + " Info:" + msg);
        }
    }
}