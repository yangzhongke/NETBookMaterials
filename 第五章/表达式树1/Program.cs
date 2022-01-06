using static System.Linq.Expressions.Expression;
using System.Linq.Expressions;
using ExpressionTreeToString;
using System.Reflection;

using var ctx = new TestDbContext();


/*
Func<Book, bool> f1 = b => b.Price > 5 || b.AuthorName.Contains("杨中科");
Expression<Func<Book, bool>> e = b => b.Price > 5 || b.AuthorName.Contains("杨中科");
Console.WriteLine(f1);
Console.WriteLine(e);*/
//ExpressionTreeToString的基本使用
/*
Expression<Func<Book, bool>> e = b => b.AuthorName.Contains("杨中科") || b.Price > 30;
Console.WriteLine(e.ToString("Object notation", "C#"));
*/
//动态创建和Expression<Func<Book, bool>> e = b =>b.Price > 5一样的代码
/*
ParameterExpression paramB = Expression.Parameter(typeof(Book), "b");
MemberExpression exprLeft = Expression.MakeMemberAccess(paramB, typeof(Book).GetProperty("Price"));
ConstantExpression exprRight = Expression.Constant(5.0, typeof(double));
BinaryExpression exprBody = Expression.MakeBinary(ExpressionType.GreaterThan, exprLeft, exprRight);
Expression<Func<Book, bool>> expr1 = Expression.Lambda<Func<Book, bool>>(exprBody, paramB);
ctx.Books.Where(expr1).ToList();
Console.WriteLine(expr1.ToString("Object notation", "C#"));
*/
//ExpressionTreeToString的Factory methods
/*
Expression<Func<Book, bool>> e = b => b.AuthorName.Contains("杨中科") || b.Price > 30;
Console.WriteLine(e.ToString("Factory methods", "C#"));
*/
//动态构建表达式书的代码
/*
using ExpressionTreeToString;
using static System.Linq.Expressions.Expression;
var b = Parameter(typeof(Book),"b");
var expr1 = Lambda<Func<Book,bool>>(OrElse(
        Call(MakeMemberAccess(b,typeof(Book).GetProperty("AuthorName")),
            typeof(string).GetMethod("Contains", new[] {typeof(string)}),
            Constant("杨中科")),
        GreaterThan(MakeMemberAccess(b,typeof(Book).GetProperty("Price")),
            Constant(30.0))
    ),b);
using TestDbContext ctx = new TestDbContext();
ctx.Books.Where(expr1).ToList();
Console.WriteLine(expr1.ToString("Object notation", "C#"));*/
/*
using ExpressionTreeToString;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;
Expression<Func<Book, bool>> expr1 = b => b.Price == 5;
Expression<Func<Book, bool>> expr2 = b => b.Title == "零基础趣学C语言";
Console.WriteLine(expr1.ToString("Factory methods", "C#"));
Console.WriteLine(expr2.ToString("Factory methods", "C#"));*/
//QueryBooks
/*
using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;
QueryBooks("Price", 18.0);
QueryBooks("AuthorName", "杨中科");
QueryBooks("Title", "零基础趣学C语言");
IEnumerable<Book> QueryBooks(string propName, object value)
{
	Type type = typeof(Book);
	PropertyInfo propInfo = type.GetProperty(propName);
	Type propType = propInfo.PropertyType;
	var b = Parameter(typeof(Book),"b");
	Expression<Func<Book,bool>> expr;
	if (propType.IsPrimitive)//如果是int、double等基本数据类型
	{
		expr = Lambda<Func<Book, bool>>(Equal(
				MakeMemberAccess(b,typeof(Book).GetProperty(propName)),
				Constant(value)),b);
	}
	else//如果是string等类型
	{
		expr = Lambda<Func<Book, bool>>(MakeBinary(ExpressionType.Equal,
				MakeMemberAccess(b,typeof(Book).GetProperty(propName)),
				Constant(value), false,propType.GetMethod("op_Equality")
			),b);
	}
	TestDbContext ctx = new TestDbContext();
	return ctx.Books.Where(expr).ToArray();
}
*/
//5.3.8	不用Emit生成IL代码实现Select的动态化
/*
using System.Linq.Expressions;
var items = Query<Book>(new string[] { "Id", "PubTime", "Title" });
foreach (object[] row in items)
{
	long id = (long)row[0];
	DateTime pubTime = (DateTime)row[1];
	string title = (string)row[2];
	Console.WriteLine(id + "," + pubTime + "," + title);
}
IEnumerable<object[]> Query<TEntity>(string[] propNames) where TEntity : class
{
	ParameterExpression exParameter = Expression.Parameter(typeof(TEntity));
	List<Expression> exProps = new List<Expression>();
	foreach (string propName in propNames)
	{
		Expression exProp = Expression.Convert(Expression.MakeMemberAccess(
			exParameter,typeof(TEntity).GetProperty(propName)), typeof(object));
		exProps.Add(exProp);
	}
	Expression[] initializers = exProps.ToArray();
	NewArrayExpression newArrayExp = Expression.NewArrayInit(typeof(object), initializers);
	var selectExpression = Expression.Lambda<Func<TEntity, object[]>>(newArrayExp, exParameter);
	using TestDbContext ctx = new TestDbContext();
	IQueryable<object[]> selectQueryable = ctx.Set<TEntity>().Select(selectExpression);
	return selectQueryable.ToArray();
}
*/
//尽量避免使用动态构建表达式树
/*
Book[] QueryBooks(string title, double? lowerPrice, double? upperPrice, int orderByType)
{
	using TestDbContext ctx = new TestDbContext();
	IQueryable<Book> source = ctx.Books;
	if (!string.IsNullOrEmpty(title))
	{
		source = source.Where(b => b.Title.Contains(title));
	}
	if (lowerPrice != null)
	{
		source = source.Where(b => b.Price >= lowerPrice);
	}
	if (upperPrice != null)
	{
		source = source.Where(b => b.Price <= upperPrice);
	}
	if (orderByType == 1)
	{
		source = source.OrderByDescending(b => b.Price);
	}
	else if (orderByType == 2)
	{
		source = source.OrderBy(b => b.Price);
	}
	return source.ToArray();
}*/

/*
string word = "C语言";
var books = ctx.Books.WhereInterpolated($"Price>8 or Title.Contains({word})");
foreach(var b in books)
{
    Console.WriteLine($"{b.Id},{b.Title},{b.Price}");
}*/
/*
var books1 = ctx.Books.Where(IsOk).ToArray();
foreach (var b in books1)
{
    Console.WriteLine(b);
}
bool IsOk(Book b)
{
	return b.Price>3;
}*/
/*
var books1 = ctx.Books.Where(b => b.Title.PadLeft(5) == "hello");
foreach (var b in books1)
{
	Console.WriteLine(b);
}*/

//ctx.Books.Where(b => b.Price > 5);
/*
Func<Book, bool> f1 = b => b.Price > 5 || b.AuthorName.Contains("杨中科");
Expression<Func<Book, bool>> e = b => b.Price > 5 || b.AuthorName.Contains("杨中科");
Console.WriteLine(f1);
Console.WriteLine(e);
*/
/*
Expression<Func<Book, bool>> e = b => b.AuthorName.Contains("杨中科") || b.Price > 30;
Console.WriteLine(e.ToString("Factory methods", "C#"));
*/
/*
Expression<Func<Book, bool>> expr1 = b => b.Price == 5;
Expression<Func<Book, bool>> expr2 = b => b.Title == "零基础趣学C语言";
Console.WriteLine(RemoveEmptyLine(expr1.ToString("Factory methods", "C#")));
Console.WriteLine(RemoveEmptyLine(expr2.ToString("Factory methods", "C#")));
string RemoveEmptyLine(string s)
{
	var strs = s.Split("\r\n",StringSplitOptions.RemoveEmptyEntries);
	return string.Join("\r\n", strs);
}*/
QueryBooks("Price", 18.0);
QueryBooks("AuthorName", "杨中科");
QueryBooks("Title", "零基础趣学C语言");
IEnumerable<Book> QueryBooks(string propName, object value)
{
	Type type = typeof(Book);
	PropertyInfo propInfo = type.GetProperty(propName);
	Type propType = propInfo.PropertyType;
	var b = Parameter(typeof(Book), "b");
	Expression<Func<Book, bool>> expr;
	if (propType.IsPrimitive)//如果是int、double等基本数据类型
	{
		expr = Lambda<Func<Book, bool>>(Equal(
				MakeMemberAccess(b, typeof(Book).GetProperty(propName)),
				Constant(value)), b);
	}
	else//如果是string等类型
	{
		expr = Lambda<Func<Book, bool>>(MakeBinary(ExpressionType.Equal,
				MakeMemberAccess(b, typeof(Book).GetProperty(propName)),
				Constant(value), false, propType.GetMethod("op_Equality")
			), b);
	}
	TestDbContext ctx = new TestDbContext();
	return ctx.Books.Where(expr).ToArray();
}
