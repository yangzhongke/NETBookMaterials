using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Data.Common;

//IEnumerable版数据查询
/*
using var ctx = new TestDbContext();
IEnumerable<Book> books = ctx.Books;
foreach (var b in books.Where(b => b.Price > 1.1))
{
	Console.WriteLine($"Id={b.Id},Title={b.Title}");
}*/
//没有遍历的查询
/*
using var ctx = new TestDbContext();
IQueryable<Book> books = ctx.Books.Where(b => b.Price > 1.1);
Console.WriteLine(books);*/
//遍历查询
/*
using var ctx = new TestDbContext();
Console.WriteLine("1、Where之前");
IQueryable<Book> books = ctx.Books.Where(b => b.Price > 1.1);
Console.WriteLine("2、遍历IQueryable之前");
foreach (var b in books)
{
	Console.WriteLine(b.Title + ":" + b.PubTime);
}
Console.WriteLine("3、遍历IQueryable之后");
*/
//拼接复杂的查询条件

//QueryBooks("爱", true, true, 30);
/*
QueryBooks("爱", false, false, 18);
void QueryBooks(string searchWords, bool searchAll, bool orderByPrice, double upperPrice)
{
	using TestDbContext ctx = new TestDbContext();
	IQueryable<Book> books = ctx.Books.Where(b => b.Price <= upperPrice);
	if (searchAll)//匹配书名或、作者名
	{
		books = books.Where(b => b.Title.Contains(searchWords) || b.AuthorName.Contains(searchWords));
	}
	else//只匹配书名
	{
		books = books.Where(b => b.Title.Contains(searchWords));
	}
	if (orderByPrice)//按照价格排序
	{
		books = books.OrderBy(b => b.Price);
	}
	foreach (Book b in books)
	{
		Console.WriteLine($"{b.Id},{b.Title},{b.Price},{b.AuthorName}");
	}
}*/
//IQueryable的复用
/*
using TestDbContext ctx = new TestDbContext();
IQueryable<Book> books = ctx.Books.Where(b => b.Price >=8);
Console.WriteLine(books.Count());
Console.WriteLine(books.Max(b => b.Price));
foreach (Book b in books.Where(b => b.PubTime.Year > 2000))
{
	Console.WriteLine(b.Title);
}*/
//分页查询
/*
OutputPage(1, 5);
Console.WriteLine("******");
OutputPage(2, 5);
void OutputPage(int pageIndex, int pageSize)
{
	using TestDbContext ctx = new TestDbContext();
	IQueryable<Book> books = ctx.Books.Where(b => !b.Title.Contains("张三"));
	long count = books.LongCount();//总条数
	long pageCount = (long)Math.Ceiling(count * 1.0 / pageSize);//页数
	Console.WriteLine("页数：" + pageCount);
	var pagedBooks = books.Skip((pageIndex - 1) * pageSize).Take(pageSize);
	foreach (var b in pagedBooks)
	{
		Console.WriteLine(b.Id + "," + b.Title);
	}
}*/
//有错误的返回IQueryable查询结果
/*
foreach (var b in QueryBooks())
{
	Console.WriteLine(b.Title);
}
IQueryable<Book> QueryBooks()
{
	using TestDbContext ctx = new TestDbContext();
	return ctx.Books.Where(b => b.Id > 5);
}*/
//正确的返回IEnumerable的数据
/*
foreach (var b in QueryBooks())
{
	Console.WriteLine(b.Title);
}
IEnumerable<Book> QueryBooks()
{
	using (TestDbContext ctx = new TestDbContext())
	{
		return ctx.Books.Where(b => b.Id > 5).ToArray();
	}
}
*/
//执行非查询SQL语句
/*
using TestDbContext ctx = new TestDbContext();
Console.WriteLine("请输入最低价格");
double price = double.Parse(Console.ReadLine());
Console.WriteLine("请输入姓名");
string aName = Console.ReadLine();
int rows = await ctx.Database.ExecuteSqlInterpolatedAsync(@$"
	insert into T_Books (Title,PubTime,Price,AuthorName)
	select Title, PubTime, Price,{aName} from T_Books where Price>{price}");
*/
//执行实体相关的查询SQL语句
/*
using TestDbContext ctx = new TestDbContext();
Console.WriteLine("请输入年份");
int year = int.Parse(Console.ReadLine());
IQueryable<Book> books = ctx.Books.FromSqlInterpolated(@$"select * from T_Books
		where DatePart(year,PubTime)>{year} order by newid()");
foreach (Book b in books)
{
	Console.WriteLine(b.Title);
}*/
//对FromSqlInterpolated结果进行二次加工
/*
using TestDbContext ctx = new TestDbContext();
int year = int.Parse(Console.ReadLine());
IQueryable<Book> books = ctx.Books.FromSqlInterpolated(@$"select * from T_Books 
		where DatePart(year,PubTime)>{year}");
foreach (Book b in books.Skip(3).Take(6))
{
	Console.WriteLine(b.Title);
}*/
//执行任意查询语句
/*
using TestDbContext ctx = new TestDbContext();
Console.WriteLine("请输入年份");
int year = int.Parse(Console.ReadLine());
DbConnection conn = ctx.Database.GetDbConnection();
if (conn.State != ConnectionState.Open)
{
	conn.Open();
}
using (var cmd = conn.CreateCommand())
{
	cmd.CommandText = @"select AuthorName,Count(*) from T_Books 
				where DatePart(year,PubTime)>@year
				group by AuthorName";
	var p1 = cmd.CreateParameter();
	p1.ParameterName = "@year";
	p1.Value = year;
	cmd.Parameters.Add(p1);
	using (var reader = cmd.ExecuteReader())
	{
		while (reader.Read())
		{
			string aName = reader.GetString(0);
			int count = reader.GetInt32(1);
			Console.WriteLine($"{aName}:{count}");
		}
	}
}*/
//对象的跟踪状态
/*
using TestDbContext ctx = new TestDbContext();
Book[] books = ctx.Books.Take(3).ToArray();
Book b1 = books[0];
Book b2 = books[1];
Book b3 = books[2];
Book b4 = new Book { Title = "零基础趣学C语言", AuthorName = "杨中科" };
Book b5 = new Book { Title = "百年孤独", AuthorName = "马尔克斯" };
b1.Title = "abc";
ctx.Remove(b3);
ctx.Add(b4);
EntityEntry entry1 = ctx.Entry(b1);
EntityEntry entry2 = ctx.Entry(b2);
EntityEntry entry3 = ctx.Entry(b3);
EntityEntry entry4 = ctx.Entry(b4);
EntityEntry entry5 = ctx.Entry(b5);
Console.WriteLine("b1.State:" + entry1.State);
Console.WriteLine("b1.DebugView:" + entry1.DebugView.LongView);
Console.WriteLine("b2.State:" + entry2.State);
Console.WriteLine("b3.State:" + entry3.State);
Console.WriteLine("b4.State:" + entry4.State);
Console.WriteLine("b5.State:" + entry5.State);
*/
//AsNoTracking
/*
using TestDbContext ctx = new TestDbContext();
Book[] books = ctx.Books.AsNoTracking().Take(3).ToArray();
Book b1 = books[0];
b1.Title = "abc";
EntityEntry entry1 = ctx.Entry(b1);
Console.WriteLine(entry1.State);
*/
/*
using TestDbContext ctx = new TestDbContext();
Book b1 = ctx.Books.Single(b => b.Id == 10);
b1.Title = "yzk";
ctx.SaveChanges();
*/
using TestDbContext ctx = new TestDbContext();
/*
Book b1 = new Book { Id = 10 };
b1.Title = "yzk";
var entry1 = ctx.Entry(b1);
entry1.Property("Title").IsModified = true;
Console.WriteLine(entry1.DebugView.LongView);
ctx.SaveChanges();*/
Book b1 = new Book { Id = 28 };
ctx.Entry(b1).State = EntityState.Deleted;
ctx.SaveChanges();

