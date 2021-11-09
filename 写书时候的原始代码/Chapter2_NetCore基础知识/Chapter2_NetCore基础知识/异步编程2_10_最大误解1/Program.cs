using System;
using System.Threading;
using System.Threading.Tasks;

namespace 异步编程2_10_最大误解1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("1-Main-ThreadId:"+Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(await CalcAsync(10000));
            Console.WriteLine("2-Main-ThreadId:" + Thread.CurrentThread.ManagedThreadId);
        }

        
        static async Task<decimal> CalcAsync(int n)
        {
            Console.WriteLine("CalcAsync-ThreadId:" + Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() => {
                Console.WriteLine("Task.Run-ThreadId:" + Thread.CurrentThread.ManagedThreadId);
                decimal result = 1;
                Random rand = new Random();
                for (int i = 0; i < n * n; i++)
                {
                    result = result + (decimal)rand.NextDouble();
                }
                return result;
            });            
        }

    }
}