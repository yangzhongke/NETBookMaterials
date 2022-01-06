using 常用集合扩展方法;

List<Employee> list = new List<Employee>();
list.Add(new Employee { Id = 1, Name = "jerry", Age = 28, Gender = true, Salary = 5000 });
list.Add(new Employee { Id = 2, Name = "jim", Age = 33, Gender = true, Salary = 3000 });
list.Add(new Employee { Id = 3, Name = "lily", Age = 35, Gender = false, Salary = 9000 });
list.Add(new Employee { Id = 4, Name = "lucy", Age = 16, Gender = false, Salary = 2000 });
list.Add(new Employee { Id = 5, Name = "kimi", Age = 25, Gender = true, Salary = 1000 });
list.Add(new Employee { Id = 6, Name = "nancy", Age = 35, Gender = false, Salary = 8000 });
list.Add(new Employee { Id = 7, Name = "zack", Age = 35, Gender = true, Salary = 8500 });
list.Add(new Employee { Id = 8, Name = "jack", Age = 33, Gender = true, Salary = 8000 });
/*
IEnumerable<Employee> list1 = list.Where(e => e.Salary > 2500 && e.Age < 35);
foreach (Employee e in list1)
{
    Console.WriteLine(e);
}*/
/*
int count1 = list.Count(e => e.Salary > 5000 || e.Age < 30);
int count2 = list.Where(e => e.Salary > 5000 || e.Age < 30).Count();
*/
/*
bool b1 = list.Any(e => e.Salary > 8000);
bool b2 = list.Where(e => e.Salary > 8000).Any();
*/
/*
Employee e1 = list.Single(e => e.Id == 6);
Console.WriteLine(e1);
Employee? e2 = list.SingleOrDefault(e => e.Id == 9);
if (e2 == null)
{
    Console.WriteLine("没有Id==9的数据");
}
else
{
    Console.WriteLine(e2);
}
Employee e3 = list.First(e => e.Age > 30);
Console.WriteLine(e3);
Employee? e4 = list.FirstOrDefault(e => e.Age > 30);
if (e4 == null)
{
    Console.WriteLine("没有大于30岁的数据");
}
else
{
    Console.WriteLine(e2);
}
Employee e5 = list.First(e => e.Salary > 9999);
*/
/*
Console.WriteLine("------按照年龄正序排列------");
var orderedItems1 = list.OrderBy(e => e.Age);
foreach (var item in orderedItems1)
{
    Console.WriteLine(item);
}
Console.WriteLine("------按照工资倒序排列------");
var orderedItems2 = list.OrderByDescending(e => e.Salary);
foreach (var item in orderedItems2)
{
    Console.WriteLine(item);
}
*/
/*
var orderedItems1 = list.Skip(2).Take(3);
foreach (var item in orderedItems1)
{
    Console.WriteLine(item);
}
*/
/*
int maxAge = list.Max(e => e.Age);
Console.WriteLine($"最大年龄:{maxAge}");
long minId = list.Min(e => e.Id);
Console.WriteLine($"最小Id:{minId}");
double avgSalary = list.Average(e => e.Salary);
Console.WriteLine($"平均工资:{avgSalary}");
int sumSalary = list.Sum(e => e.Salary);
Console.WriteLine($"工资总和:{sumSalary}");
int count = list.Count();
Console.WriteLine($"总条数:{count}");
int minSalary2 = list.Where(e => e.Age > 30).Min(e => e.Salary);
Console.WriteLine($"大于30岁的人群中的最低工资:{minSalary2}");
*/
/*
int[] scores = { 61, 90, 100, 99, 18, 22, 38, 66, 80, 93, 55, 50, 89 };
int minScore = scores.Min();
Console.WriteLine($"最低成绩：{minScore}");
double avgScore1 = scores.Where(i => i >= 60).Average();
Console.WriteLine($"合格成绩中的平均值：{avgScore1}");
*/
/*
IEnumerable<IGrouping<int, Employee>> items = list.GroupBy(e => e.Age);
foreach (IGrouping<int, Employee> item in items)
{
    int age = item.Key;
    int count = item.Count();
    int maxSalary = item.Max(e => e.Salary);
    double avgSalary = item.Average(e => e.Salary);
    Console.WriteLine($"年龄{item.Key},人数{count},最高工资{maxSalary},平均工资{avgSalary}");
}
*/
/*
var items = list.GroupBy(e => e.Gender);
foreach (var item in items)
{
    bool gender = item.Key;
    int count = item.Count();
    double avgSalary = item.Average(e => e.Salary);
    int minAge = item.Min(e => e.Age);
    Console.WriteLine($"性别{gender},人数{count},平均工资{avgSalary:F},最小年龄{minAge}");
}
*/
/*
IEnumerable<int> ages = list.Select(e => e.Age);
Console.WriteLine(string.Join(",", ages));
IEnumerable<string> names = list.Select(e => e.Gender ? "男" : "女");
Console.WriteLine(string.Join(",", names));
*/
/*
var items = list.Select(e => new { e.Name, e.Age, XingBie = e.Gender ? "男" : "女" });
foreach (var item in items)
{
    string name = item.Name;
    int age = item.Age;
    string xingbie = item.XingBie;
    Console.WriteLine($"名字={name},年龄={age},性别={xingbie}");
}*/
/*
var items = list.GroupBy(e => e.Gender).Select(g => new {
    Gender = g.Key,
    Count = g.Count(),
    AvgSalary = g.Average(e => e.Salary),
    MinAge = g.Min(e => e.Age)
});
foreach (var item in items)
{
    Console.WriteLine($"性别{item.Gender},人数{item.Count},平均工资{item.AvgSalary:F},最小年龄{item.MinAge}");
}
*/

var items = list.Where(e => e.Id > 2).GroupBy(e => e.Age).OrderBy(g => g.Key).Take(3)
    .Select(g => new { Age = g.Key, Count = g.Count(), AvgSalary = g.Average(e => e.Salary) });
foreach (var item in items)
{
    Console.WriteLine($"年龄:{item.Age},人数：{item.Count},平均工资:{item.AvgSalary}");
}
/*
var items2 = from e in list
            where e.Salary > 3000
            orderby e.Age
            select new { e.Name, e.Age, Gender = e.Gender ? "男" : "女" };
*/