using 例子服务接口1;

namespace 例子服务实现1
{
    public class CnService : IMyService
    {
        public void SayHello()
        {
            Console.WriteLine("你好");
        }
    }
}