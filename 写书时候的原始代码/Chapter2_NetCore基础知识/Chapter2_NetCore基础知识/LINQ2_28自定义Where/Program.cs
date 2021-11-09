using System;
using System.Collections.Generic;

namespace LINQ2_28自定义Where
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arrays = { 2,8,29,19,12,13,99,89,105,108,81};
            /*
            var nums2 = MyWhere(arrays, n =>n>30);
            Console.WriteLine(string.Join(",",nums2));
            var nums3 = MyWhere(arrays, n => n%2==0);
            Console.WriteLine(string.Join(",", nums3));*/

            Func<int, bool> f1 = delegate (int n) {
                return n > 30;
            };
            var nums2 = MyWhere(arrays, f1);
            Console.WriteLine(string.Join(",", nums2));
        }
        /// <summary>
        /// 按照filter过滤条件对nums中的数据进行过滤
        /// </summary>
        /// <param name="nums">待过滤的数据</param>
        /// <param name="filter">过滤条件，参数是待判断的数据，返回值表示这个数据是否符合要求</param>
        /// <returns>过滤后的数据</returns>

        static IEnumerable<int> MyWhere(IEnumerable<int> nums,Func<int,bool> filter)
        {
            foreach(int n in nums)
            {
                //判断如果遍历到的这条数据满足要求，则把数据返回
                if(filter(n))
                {
                    yield return n;
                }
            }
        }
    }
}