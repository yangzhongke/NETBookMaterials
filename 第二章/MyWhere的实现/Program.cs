/*
int[] arrays = { 2, 8, 29, 19, 12, 13, 99, 89, 105, 108, 81 };
var nums2 = MyWhere(arrays, n => n > 30);
Console.WriteLine(string.Join(",", nums2));
var nums3 = MyWhere(arrays, n => n % 2 == 0);
Console.WriteLine(string.Join(",", nums3));
static IEnumerable<int> MyWhere(IEnumerable<int> nums, Func<int, bool> filter)
{
	foreach (int n in nums)
	{
		if (filter(n)) yield return n;
	}
}*/
int[] arrays = { 2, 8, 29, 19, 12, 13, 99, 89, 105, 108, 81 };
var nums2 = MyWhere(arrays, n => n > 30);
Console.WriteLine(string.Join(",", nums2));
var nums3 = MyWhere(arrays, n => n % 2 == 0);
Console.WriteLine(string.Join(",", nums3));
static IEnumerable<int> MyWhere(IEnumerable<int> nums, Func<int, bool> filter)
{
	foreach (int n in nums)
	{
		if (filter(n)) yield return n;
	}
}
