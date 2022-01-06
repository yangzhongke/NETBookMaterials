//数据的插入
/*
using TestDbContext ctx = new TestDbContext();
var b1 = new Book{ AuthorName = "杨中科", Title = "零基础趣学C语言", 
    Price = 59.8, PubTime = new DateTime(2019, 3, 1) };
var b2 = new Book{ AuthorName = "Robert Sedgewick", Title = "算法(第4版)", 
    Price = 99, PubTime = new DateTime(2012, 10, 1) };
var b3 = new Book{ AuthorName = "吴军", Title = "数学之美", 
    Price = 69, PubTime = new DateTime(2020, 5, 1) };
var b4 = new Book{ AuthorName = "杨中科", Title = "程序员的SQL金典", 
    Price = 52, PubTime = new DateTime(2008, 9, 1) };
var b5 = new Book{ AuthorName = "吴军", Title = "文明之光", 
    Price = 246, PubTime = new DateTime(2017, 3, 1) };
ctx.Books.Add(b1);
ctx.Books.Add(b2);
ctx.Books.Add(b3);
ctx.Books.Add(b4);
ctx.Books.Add(b5);
await ctx.SaveChangesAsync();*/
//查询
/*
using TestDbContext ctx = new TestDbContext();
Console.WriteLine("***所有书籍***");
foreach (Book b in ctx.Books)
{
    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
}
Console.WriteLine("***所有价格高于80元的书籍***");
IEnumerable<Book> books2 = ctx.Books.Where(b => b.Price > 80);
foreach (Book b in books2)
{
    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
}*/
//Single、FirstOrDefault查询
/*
using TestDbContext ctx = new TestDbContext();
Book b1 = ctx.Books.Single(b => b.Title == "零基础趣学C语言");
Console.WriteLine($"Id={b1.Id},Title={b1.Title},Price={b1.Price}");
Book? b2 = ctx.Books.FirstOrDefault(b => b.Id == 9);
if (b2 == null)
{
    Console.WriteLine("没有Id=9的数据");
}
else
{
    Console.WriteLine($"Id={b2.Id},Title={b2.Title},Price={b2.Price}");
}*/
//排序
/*
using TestDbContext ctx = new TestDbContext();
IEnumerable<Book> books = ctx.Books.OrderByDescending(b => b.Price);
foreach (Book b in books)
{
    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
}*/
//GroupBy

using TestDbContext ctx = new TestDbContext();
var groups = ctx.Books.GroupBy(b => b.AuthorName)
    .Select(g => new { AuthorName = g.Key, BooksCount = g.Count(), MaxPrice = g.Max(b => b.Price) });
foreach (var g in groups)
{
    Console.WriteLine($"作者:{g.AuthorName},图书数量:{g.BooksCount},最高价格:{g.MaxPrice}");
}
//修改数据
/*
using TestDbContext ctx = new TestDbContext();
var b = ctx.Books.Single(b => b.Title == "数学之美");
b.AuthorName = "Jun Wu";
await ctx.SaveChangesAsync();
*/
//删除数据
/*
using TestDbContext ctx = new TestDbContext();
var b = ctx.Books.Single(b => b.Title == "数学之美");
ctx.Remove(b);//也可以写成ctx.Books.Remove(b);
await ctx.SaveChangesAsync();*/
/*
using TestDbContext ctx = new TestDbContext();
Book b = new Book
{
    AuthorName = "Zack Yang",
    Title = "Zack, Cool guy!",
    Price = 9.9,
    PubTime = new DateTime(2020, 12, 30)
};
ctx.Books.Add(b);
Console.WriteLine($"保存前，Id={b.Id}");
await ctx.SaveChangesAsync();
Console.WriteLine($"保存后，Id={b.Id}");*/
