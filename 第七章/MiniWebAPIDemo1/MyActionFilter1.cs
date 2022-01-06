using MiniWebAPI;

namespace MiniWebAPIDemo1
{
    public class MyActionFilter1 : IMyActionFilter
    {
        public void Execute()
        {
            Console.WriteLine("Filter 1执行啦");
        }
    }
}
