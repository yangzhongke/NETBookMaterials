using ExpressionTreeToString;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;
using System.Linq.Dynamic.Core;

namespace EFCoreDemo1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                /*
                IQueryable<Book> q1 = ctx.Books.Where(b=>b.Price>5);
                q1 = q1.Where(b=>b.AuthorName=="杨中科");
                foreach(var b in q1)
                {
                    Console.WriteLine(b);
                }*/
                //foreach (var b in QueryDynamic(null, 3, 200, 1))
                /*
                foreach (var b in QueryDynamic("西游记", null, 200, 2))
                {
                    Console.WriteLine(b);
                }*/
                double price = 5;
                string name = "aa";
                
                ctx.Books.Where($"Price>={price} and Price<=60")
                    .Select("new(Title,Price)").ToDynamicArray();
            }
        }

        static Book[] QueryDynamic(string title,double? lowPrice,double? upPrice,
            int orderType)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                IQueryable<Book> books = ctx.Books;
                if(title!=null)
                {
                    books = books.Where(b=>b.Title==title);
                }
                if(lowPrice!=null)
                {
                    books = books.Where(b => b.Price>=lowPrice);
                }
                if (upPrice != null)
                {
                    books = books.Where(b => b.Price <= upPrice);
                }
                switch(orderType)
                {
                    case 1:
                        books = books.OrderByDescending(b=>b.Price);
                        break;
                    case 2:
                        books = books.OrderBy(b => b.Price);
                        break;
                }
                return books.ToArray();
            }
        }

        static async Task Main10(string[] args)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                /*
                var books = ctx.Books.Select(b => new {b.AuthorName,b.Price});
                foreach(var b in books)
                {
                    Console.WriteLine(b.Price+","+b.AuthorName);
                }*/
                /*
                var books = ctx.Books.Select(b=>new object[] { b.Title,b.Price});
                foreach(var b in books)
                {
                    Console.WriteLine(b[0]+","+b[1]);
                }*/
                var items = Query9<Book>("Id","Price","Title");
                foreach(var e in items)
                {
                    Console.WriteLine(e[0]+","+e[1]);
                }
            }
        }

        static IEnumerable<object[]> Query9<T>(params string[] propertyNames)
            where T:class
        {
            var p = Parameter(typeof(T));
            List<Expression> propExprList = new List<Expression>();
            foreach(var propName in propertyNames)
            {
                Expression propExpr = Convert(MakeMemberAccess(p, 
                    typeof(T).GetProperty(propName)),typeof(object));
                propExprList.Add(propExpr);
            }
            var newArrayExpr = NewArrayInit(typeof(object), propExprList.ToArray());
            var selectExpr = Lambda<Func<T, object[]>>(newArrayExpr,p);
            using (TestDbContext ctx = new TestDbContext())
            {
                return ctx.Set<T>().Select(selectExpr).ToArray();
            }
        }

        static async Task Main9(string[] args)
        {
            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                Expression<Func<Book, bool>> e1 = e => e.AuthorName =="杨中科";
                //Expression<Func<Book, bool>> e1 = e => e.Price == 5;
                Console.WriteLine(e1.ToString("Factory Methods","C#"));
            }*/
            //var books = QueryBooks2("Price", 18);
            var books = QueryBooks2("AuthorName", "杨中科");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        static IEnumerable<Book> QueryBooks2(string propertyName, object value)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                /*
                Expression<Func<Book, bool>> e1 = e => e.AuthorName == "杨中科";
                return ctx.Books.Where(e1).ToList();*/
                Expression<Func<Book, bool>> e1;
                var e = Parameter(
                    typeof(Book),
                    "e"
                    );
                var memberAccessExpr = MakeMemberAccess(e,
                                typeof(Book).GetProperty(propertyName)
                            );
                Type valueType = typeof(Book).GetProperty(propertyName).PropertyType;
                var valueConstExpr = Constant(System.Convert.ChangeType(value, valueType));
                Expression body;
                if(valueType.IsPrimitive)//原始类型
                {
                    body = Equal(
                            memberAccessExpr,
                            valueConstExpr
                        );
                }
                else
                {
                    body= MakeBinary(ExpressionType.Equal,
                            memberAccessExpr,
                            valueConstExpr, false,
                            typeof(string).GetMethod("op_Equality")
                        );
                }
                e1 = Lambda<Func<Book, bool>>(body,e);
                return ctx.Books.Where(e1).ToList();
            }
        }

        static async Task Main8(string[] args)
        {
            /*
            using (TestDbContext ctx = new TestDbContext())
            {
                var items = ctx.Books.Select(b=>new object[] { b.Id,b.Title}).ToArray();
                foreach(object[] book in items)
                {
                    Console.WriteLine("Id="+book[0]+",Title="+book[1]);
                }
            }
            ExportBooksAsTxt<Book>(new string[] { "Id", "Title" });*/
            /*
            var items = Query<Book>(new string[] { "Id","PubTime", "Title" });
            foreach(object[] row in items)
            {
                long id = (long)row[0];
                DateTime pubTime = (DateTime)row[1];
                string title = (string)row[2];
                Console.WriteLine(id+","+pubTime+","+title);
            }*/         
            
        }

        private static Book[] QueryBooks(string title, double? lowerPrice, double? upperPrice, int orderByType)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                ctx.Books.Where($"xxx");
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
                else if (orderByType == 3)
                {
                    source = source.OrderByDescending(b => b.PubTime);
                }
                else if (orderByType == 4)
                {
                    source = source.OrderBy(b => b.PubTime);
                }
                return source.ToArray();
            }
        }

        private static IEnumerable<object[]> Query<TEntity>(string[] propNames) where TEntity:class
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                ParameterExpression exParameter = Expression.Parameter(typeof(TEntity));
                //各个属性的表达式
                List<Expression> exProps = new List<Expression>();
                foreach(string propName in propNames)
                {
                    Expression exProp = Expression.Convert(Expression.MakeMemberAccess(exParameter, typeof(TEntity).GetProperty(propName)), typeof(object));
                    exProps.Add(exProp);
                }
                Expression[] initializers = exProps.ToArray();
                //数组对象对应的表达式
                NewArrayExpression newArrayExp = Expression.NewArrayInit(
                    typeof(object), initializers);
                var selectExpression = Expression.Lambda<Func<TEntity, object[]>>(newArrayExp, exParameter);
                IQueryable<object[]> selectQueryable = ctx.Set<TEntity>().Select(selectExpression);
                return selectQueryable.ToArray();
            }
        }

        static async Task Main7(string[] args)
        {
            var lines = File.ReadAllLines(@"E:\主同步盘\我的坚果云\UoZ\SE101-玩着学编程\Part4课件\练习：统计总播放量微博播放量.txt");
            int sum = 0;
            foreach (var line in lines)
            {
                int indexOfCi = line.IndexOf("次");
                if (indexOfCi >= 0)
                {
                    string sCount = line.Substring(0, indexOfCi);
                    Console.WriteLine(sCount);
                    sum += int.Parse(sCount);
                }
            }
            Console.WriteLine(sum);
            /*
            Expression<Func<Book, bool>> expr1 = b => b.Price == 5;
            Expression<Func<Book, bool>> expr2 = b => b.Title == "零基础趣学C语言";
            Console.WriteLine(expr1.ToString("Factory methods", "C#"));
            Console.WriteLine(expr2.ToString("Factory methods", "C#"));*/
            /*
            Expression<Func<Book, bool>> expr1 = b => b.Title.Contains("A")||b.AuthorName.Contains("A") || b.AuthorName.Contains("A");
            Console.WriteLine(expr1.ToString("Factory methods", "C#"));*/
            /*
            QueryBooks("Price", 18.0);
            QueryBooks("AuthorName", "杨中科");
            QueryBooks("Title", "零基础趣学C语言");*/
        }

        static IEnumerable<Book> QueryBooks(string propName,object value)
        {
            Type type = typeof(Book);
            PropertyInfo propInfo = type.GetProperty(propName);
            if(propInfo==null)
            {
                throw new ArgumentException($"property ${propName} not found");
            }
            Type propType = propInfo.PropertyType;
            var b = Parameter(
                typeof(Book),
                "b"
            );
            LambdaExpression expr;
            if(propType.IsPrimitive)
            {
                expr = Lambda(
                    Equal(
                        MakeMemberAccess(b,
                            typeof(Book).GetProperty(propName)
                        ),
                        Constant(value)
                    ),
                    b
                );
            }
            else
            {
                expr = Lambda(
                    MakeBinary(ExpressionType.Equal,
                        MakeMemberAccess(b,
                            typeof(Book).GetProperty(propName)
                        ),
                        Constant(value), false,
                        propType.GetMethod("op_Equality")
                    ),
                    b
                );
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                return ctx.Books.Where(expr).ToArray();
            }
        }

        static async Task Main3(string[] args)
        {
            using (TestDbContext ctx = new TestDbContext())
            {

                //无法将具有语句体的lambda表达式转换为表达式树
                //Expression<Func<Book, bool>> e1 = b => { return b.Price > 5};
                //Expression<Func<Book, bool>> e = b =>b.Price > 5;
                //ctx.Books.Where(e1.Compile()).ToArray();//客户端评估
                // ctx.Books.Where(e).ToList();//服务器端评估
                /*
                Func<Book, bool> e = b => b.Price > 5;
                ctx.Books.Where(e).ToList();*/
                //ctx.Books.Where(b => b.Price > 5).ToList();
                /*
                Func<Book, bool> f1 = b => b.Price > 5||b.AuthorName.Contains("杨中科");
                Expression<Func<Book, bool>> e = b => b.Price > 5 || b.AuthorName.Contains("杨中科");
                Console.WriteLine(f1);
                Console.WriteLine(e);*/
                //Install-Package ExpressionTreeToString
                //Console.WriteLine(e1.ToString("Object notation", "C#"));//using ExpressionTreeToString;
                /*
                ParameterExpression paramB = Expression.Parameter(typeof(Book),"b");
                MemberExpression exprLeft = Expression.MakeMemberAccess(paramB, typeof(Book).GetProperty("Price"));
                ConstantExpression exprRight = Expression.Constant(5.0,typeof(double));
                BinaryExpression exprBody = Expression.MakeBinary(ExpressionType.GreaterThan, exprLeft, exprRight);
                Expression<Func<Book, bool>> expr1 = Expression.Lambda<Func<Book, bool>>(exprBody, paramB);
                ctx.Books.Where(expr1).ToList();
                Console.WriteLine(expr1.ToString("Object notation", "C#"));*/
                //Console.WriteLine(expr1.ToString("Factory methods", "C#"));
                //ctx.Books.Where(expr1).ToList();
                /*
                var b = Parameter(
                    typeof(Book),
                    "b"
                );

                var expr1 = Lambda(
                    OrElse(
                        Call(
                            MakeMemberAccess(b,
                                typeof(Book).GetProperty("AuthorName")
                            ),
                            typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                            Constant("杨中科")
                        ),
                        GreaterThan(
                            MakeMemberAccess(b,
                                typeof(Book).GetProperty("Price")
                            ),
                            Constant(30)
                        )
                    ),
                    b
                );
                ctx.Books.Where(expr1).ToList();
                Console.WriteLine(expr1.ToString("Object notation", "C#"));*/
                ///Expression<Func<Book, bool>> e2 = b => b.AuthorName.Contains("杨中科")||b.Price>30;
                //Console.WriteLine(e2.ToString("Factory methods", "C#"));

                //ctx.Books.Where(e3).ToList();

                //如果不加Convert，就会报错：Expression of type 'System.Int64' cannot be used for return type 'System.Object'
                //Value types need to be boxed to be seen as objects. The compiler does it for you normally, but if you build code yourself (e.g expression trees), you need to specify it as an explicit conversion
                /*
                Expression[] initializers = new Expression[] { Expression.Convert(Expression.MakeMemberAccess(b, typeof(Book).GetProperty("Id")),typeof(object)), Expression.Convert(Expression.MakeMemberAccess(b, typeof(Book).GetProperty("Title")), typeof(object)) };
                ParameterExpression paramB = Expression.Parameter(typeof(Book), "b");
                Expression[] initializers = new Expression[] { Expression.Convert(Expression.Property(paramB, typeof(Book).GetProperty("Id")), typeof(object)), Expression.Convert(Expression.Property(paramB, typeof(Book).GetProperty("Title")), typeof(object)) };

                NewArrayExpression newArrayExp = Expression.NewArrayInit(
                    typeof(object), initializers);
                var selectExpression = Expression.Lambda<Func<Book, object>>(newArrayExp, paramB);
                IQueryable<object> selectQueryable = ctx.Books.Select(selectExpression);
                selectQueryable.ToList();*/

                //https://github.com/zzzprojects/System.Linq.Dynamic.Core
                //Install-Package Microsoft.EntityFrameworkCore.DynamicLinq
                /*
                var query = ctx.Books
                .Where("Id > 0 and !Title.Contains(@0)", "杨中科")
                .OrderBy("Price")
                .Select("new(Id,Title)");
                var items = query.ToDynamicArray();
                foreach(var item in items)
                {
                    Console.WriteLine(item.Id+","+item.Title);
                }*/
            }
        }
        /*
        static async Task Main2(string[] args)
        {
            var lines = File.ReadAllLines(@"E:\主同步盘\我的坚果云\UoZ\SE101-玩着学编程\Part4课件\练习：头条视频播放量统计.txt");
            int sum = 0;
            foreach (var line in lines)
            {
                int indexOf播放 = line.IndexOf("播放");
                int indexOf点赞 = line.IndexOf("点赞");
                if (indexOf播放 >= 0&& indexOf点赞 > 0)
                {
                    string sCount = line.Substring(indexOf播放 + 3, indexOf点赞 - indexOf播放 - 3).Replace(",","");
                    Console.WriteLine(sCount);
                    sum += int.Parse(sCount);
                }
            }
            Console.WriteLine("xxxxxxxxx" + sum);
            var lines = File.ReadAllLines(@"C:\Users\cowne\Desktop\微博播放量.txt");
            int sum = 0;
            foreach(var line in lines)
            {
                int indexOfCi = line.IndexOf("次");
                if(indexOfCi>=0)
                {
                    string sCount = line.Substring(0, indexOfCi);
                    Console.WriteLine(sCount);
                    sum += int.Parse(sCount);
                }                
            }
            Console.WriteLine("xxxxxxxxx"+sum);
            using (TestDbContext ctx = new TestDbContext())
            {

            }
        }
        static async Task Main1(string[] args)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                ctx.RemoveRange(ctx.Books.Where(b => b.Price > 33));
                ctx.SaveChanges();
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                Book b1 = new Book { AuthorName = "杨中科", Title = "零基础趣学C语言", Price = 59.8, PubTime = new DateTime(2019, 3, 1) };
                Book b2 = new Book { AuthorName = "Robert Sedgewick", Title = "算法(第4版)", Price = 99, PubTime = new DateTime(2012, 10, 1) };
                Book b3 = new Book { AuthorName = "吴军", Title = "数学之美", Price = 69, PubTime = new DateTime(2020, 5, 1) };
                Book b4 = new Book { AuthorName = "杨中科", Title = "程序员的SQL金典", Price = 52, PubTime = new DateTime(2008, 9, 1) };
                Book b5 = new Book { AuthorName = "吴军", Title = "文明之光", Price = 246, PubTime = new DateTime(2017, 3, 1) };
                ctx.Books.Add(b1);
                ctx.Books.Add(b2);
                ctx.Books.Add(b3);
                ctx.Books.Add(b4);
                ctx.Books.Add(b5);
                await ctx.SaveChangesAsync();
            } 
            using (TestDbContext ctx = new TestDbContext())
            {
                Console.WriteLine("***所有书籍***");
                foreach (Book b in ctx.Books)
                {
                    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
                }
                Console.WriteLine("***所有价格高于80元的书籍***");
                IEnumerable<Book> books2 = ctx.Books.Where(b => b.Price > 80);
                foreach(Book b in books2)
                {
                    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
                }
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                Book b1 = ctx.Books.Single(b => b.Title== "零基础趣学C语言");
                Console.WriteLine($"Id={b1.Id},Title={b1.Title},Price={b1.Price}");

                Book b2 = ctx.Books.FirstOrDefault(b=>b.Id==9);
                if(b2==null)
                {
                    Console.WriteLine("没有Id=9的数据");
                }
                else
                {
                    Console.WriteLine($"Id={b2.Id},Title={b2.Title},Price={b2.Price}");
                }
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                IEnumerable<Book> books = ctx.Books.OrderByDescending(b => b.Price);
                foreach (Book b in books)
                {
                    Console.WriteLine($"Id={b.Id},Title={b.Title},Price={b.Price}");
                }
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                var groups = ctx.Books.GroupBy(b => b.AuthorName)
                    .Select(g => new { AuthorName = g.Key, BooksCount = g.Count(), MaxPrice = g.Max(b => b.Price) });
                foreach(var g in groups)
                {
                    Console.WriteLine($"作者名:{g.AuthorName},著作数量:{g.BooksCount},最贵的价格:{g.MaxPrice}");
                }
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                var b = ctx.Books.Single(b=>b.Title== "数学之美");
                b.AuthorName = "Jun Wu";
                await ctx.SaveChangesAsync();
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                var b = ctx.Books.Single(b => b.Title == "数学之美");
                ctx.Remove(b);//也可以写成ctx.Books.Remove(b);
                await ctx.SaveChangesAsync();
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                var books = ctx.Books.Where(b=>b.Price<100);
                foreach(var b in books)
                {
                    b.Price += 1;
                }
                ctx.SaveChanges();
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                Book b = new Book { AuthorName = "Bill Gates", Title = "Zack, Cool guy!", Price =9.9, PubTime = new DateTime(2020, 12, 30) };
                b.Id = 6;
                ctx.Books.Add(b);
                Console.WriteLine($"保存前，Id={b.Id}");
                await ctx.SaveChangesAsync();
                Console.WriteLine($"保存后，Id={b.Id}");
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                Console.WriteLine("****1*****");
                Author a1 = new Author { Name="杨中科"};
                Console.WriteLine($"Add前，Id={a1.Id}");
                ctx.Authors.Add(a1);
                Console.WriteLine($"Add后，保存前，Id={a1.Id}");
                await ctx.SaveChangesAsync();
                Console.WriteLine($"保存后，Id={a1.Id}");

                Console.WriteLine("****2*****");
                Author a2 = new Author { Name = "Zack Yang" };
                a2.Id = Guid.NewGuid();
                Console.WriteLine($"保存前，Id={a2.Id}");                
                ctx.Authors.Add(a2);
                await ctx.SaveChangesAsync();
                Console.WriteLine($"保存前，Id={a2.Id}");

                var sql = ctx.Database.GenerateCreateScript();
                Console.WriteLine(sql);
            }
            int[] nums = { 3,5,933,2,69,69,11};
            IEnumerable<int> items = nums.Where(n => n > 10);
            using (TestDbContext ctx = new TestDbContext())
            {
                IQueryable<Book> books = ctx.Books;
                //IEnumerable<Book> books = ctx.Books;
                //IQueryable<Book> books = ctx.Books.Where(b => b.Price > 1.1 );
               foreach (var b in books.Where(b => b.Price > 1))
                {
                    Console.WriteLine($"Id={b.Id},Title={b.Title}");
                }
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                //IQueryable<Book> books = ctx.Books.Where(b=>b.Price>1.1);
                IEnumerable<Book> books = ctx.Books.Where(b => b.Price > 1.1);
                var items = books.Select(b=>new {TitlePre=b.Title.Substring(0,2),PubYear=b.PubTime.Year});
                foreach(var item in items)
                {
                    Console.WriteLine(item.TitlePre+":"+item.PubYear);
                }
            
            using (TestDbContext ctx = new TestDbContext())
            {
                Console.WriteLine("1、Where之前");
                IQueryable<Book> books = ctx.Books.Where(b=>b.Price>1.1);
                books.Skip(2).Take(5).ToArray();
                Console.WriteLine("2、遍历IQueryable之前");
                foreach (var b in books)
                {
                    Console.WriteLine(b.Title + ":" + b.PubTime);
                }
                Console.WriteLine("3、遍历IQueryable之后");
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                IEnumerable<Book> books = ctx.Books.Where(b => b.Price > 1.1);
                var items = books.Select(b => new { TitlePre = b.Title.Substring(0, 2), PubYear = b.PubTime.Year });
                foreach (var item in items)
                {
                    Console.WriteLine(item.TitlePre + ":" + item.PubYear);
                }
            }
            //QueryBooks("爱", true, true, 30);
            //QueryBooks("爱", false, false, 18);
            using (TestDbContext ctx = new TestDbContext())
            {
                IQueryable<Book> books = ctx.Books.Where(b => b.Price <= 8);
                Console.WriteLine(books.Count());
                Console.WriteLine(books.Max(b=>b.Price));
                foreach(Book b in books.Where(b=>b.PubTime.Year>2000))
                {
                    Console.WriteLine(b.Title);
                }
            }
            using (TestDbContext ctx = new TestDbContext())
            {
                var books = ctx.Books.Where(b=>b.PubTime.Year>2010).Take(3);
                foreach(var b in books)
                {
                    Console.WriteLine(b.Title);
                }
            }
            //InsertFakerBooks();
            using (TestDbContext ctx = new TestDbContext())
            {
                var books = ctx.Books.Skip(3).Take(5);
                foreach (var b in books)
                {
                    Console.WriteLine(b.Id+","+b.Title);
                }`
            }
            OutputPage(1,5);
            Console.WriteLine("******");
            OutputPage(2, 5);
            //InsertFakerBooks();
            string connStr = "Persist Security Info=False;User ID=sa;Password=123456;Initial Catalog=demo1;Server=192.168.142.128";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "select * from T_Books";
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string title = reader.GetString(reader.GetOrdinal("Title"));
                Console.WriteLine(title);
                //Thread.Sleep(2000);
            }
            
            using (TestDbContext ctx = new TestDbContext())
            {
                var books = await ctx.Books.Take(500000).ToListAsync();
                foreach (var b in books)
                {
                    Console.WriteLine(b.Id + "," + b.Title);
                }
            }
            
            foreach(var b in QueryBooks())
            {
                Console.WriteLine(b.Title);
            }
            
            using (TestDbContext ctx = new TestDbContext())
            {
                var books = ctx.Books.Where(b=>b.Id>1).ToList();
                foreach (var b in books)
                {
                    Console.WriteLine(b.Id + "," + b.Title);
                    foreach(var a in ctx.Authors)
                    {
                        Console.WriteLine(a.Id);
                    }
                }
            }
            
            using (TestDbContext ctx = new TestDbContext())
            {
                foreach(Book b in await ctx.Books.ToListAsync())
                {
                    Console.WriteLine(b.Title);
                }
            }
            
            using (TestDbContext ctx = new TestDbContext())
            {
                await foreach (Book b in ctx.Books.AsAsyncEnumerable())
                {
                    Console.WriteLine(b.Title);
                }
            }

            using (TestDbContext ctx = new TestDbContext())
            {

                
                Book b = ctx.Books.First();
                b.Price = b.Price + 1;
                //ctx.ChangeTracker.
                ctx.SaveChanges();


                
                Console.WriteLine("请输入最低价格");
                double price = double.Parse(Console.ReadLine());
                Console.WriteLine("请输入姓名");
                string aName = Console.ReadLine();
                int rows = await ctx.Database.ExecuteSqlRawAsync(@$"insert into T_Books(Title,PubTime,Price,AuthorName)
                                select Title, PubTime, Price,{aName} from T_Books
                                where Price > {price}");
                Console.WriteLine(rows);

                
                Console.WriteLine("请输入年份");
                int year = int.Parse(Console.ReadLine());
                IQueryable<Book> books = ctx.Books.FromSqlInterpolated(@$"select * from T_Books
                    where DatePart(year,PubTime)>{year}");
                foreach(Book b in books.Skip(3).Take(6))
                {
                    Console.WriteLine(b.Title);
                }
                
                Console.WriteLine("请输入年份");
                int year = int.Parse(Console.ReadLine());
                var items = ctx.Books.Where(b=>b.PubTime.Year>year).GroupBy(b => b.AuthorName).Select(g=>new { AName=g.Key,Count=g.Count()});
                foreach(var item in items)
                {
                    Console.WriteLine(item.AName+":"+item.Count);
                }
                
                DbConnection conn = ctx.Database.GetDbConnection();
                conn.Open();
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
                        while(reader.Read())
                        {
                            string aName = reader.GetString(0);
                            int count = reader.GetInt32(1);
                            Console.WriteLine($"{aName}:{count}");
                        }
                    }
                }


                //ctx.Database.ExecuteSqlRawAsync
                //ctx.Books.FromSqlInterpolated()
                //ctx.Database.GetDbConnection()
                
                Book[] books = ctx.Books.Take(3).ToArray();
                Book b1 = books[0];
                Book b2 = books[1];
                Book b3 = books[2];
                Book b4 = new Book { Title="零基础趣学C语言",AuthorName="杨中科"};
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
                
                Book[] books = ctx.Books.AsNoTracking().Take(3).ToArray();
                Book b1 = books[0];
                b1.Title = "abc";
                EntityEntry entry1 = ctx.Entry(b1);
                Console.WriteLine(entry1.State);
                
                Book b1 = new Book {Id=10};
                b1.Title = "yzk";
                var entry1 = ctx.Entry(b1);
                entry1.Property("Title").IsModified = true;
                Console.WriteLine(entry1.DebugView.LongView);
                ctx.SaveChanges();
                
                Book b1 = new Book { Id = 28 };
                ctx.Entry(b1).State = EntityState.Deleted;
                ctx.SaveChanges();
                //ctx.Books.Where(b=>b.Price>20).ToArray();
                //ctx.Books.IgnoreQueryFilters().Where(b => b.Title.Contains("o")).ToArray();
            }
        }

        static void PrintPropertyValues(PropertyValues values)
        {
            foreach (var prop in values.Properties)
            {
                Console.WriteLine(prop.Name+"="+values[prop]);
            }
        }

        static IEnumerable<Book> QueryBooks()
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                return ctx.Books.Where(b=>b.Id>5).ToArray();
            }
        }

        private static void OutputPage(int pageIndex,int pageSize)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                IQueryable<Book> books = ctx.Books.Where(b => !b.Title.Contains("张三"));
                long count = books.LongCount();//总条数
                long pageCount = (long)Math.Ceiling(count * 1.0 / pageSize);//页数
                Console.WriteLine("页数："+pageCount);
                //分页获取数据
                var pagedBooks = books.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                foreach(var b in pagedBooks)
                {
                    Console.WriteLine(b.Id+","+b.Title);
                }
            }
        }

        private static void InsertFakerBooks()
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                for(int i=0;i<1000;i++)
                {
                    Book b = new Book();
                    b.AuthorName = Faker.Name.First();
                    b.Price = Faker.RandomNumber.Next();
                    b.PubTime = Faker.Identification.DateOfBirth();
                    b.Title = Faker.Name.First();
                    ctx.Books.Add(b);
                    ctx.SaveChanges();
                }
                
            }
        }

        private static void QueryBooks(string searchWords, bool searchAll, bool orderByPrice,
            double upperPrice)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                IQueryable<Book> books = ctx.Books.Where(b=>b.Price<=upperPrice);
                if(searchAll)//匹配书名或、作者名
                {
                    books = books.Where(b=>b.Title.Contains(searchWords)|| b.AuthorName.Contains(searchWords));
                }
                else//只匹配书名
                {
                    books = books.Where(b => b.Title.Contains(searchWords));
                }
                if(orderByPrice)//按照价格排序
                {
                    books = books.OrderBy(b=>b.Price);
                }
                foreach(Book b in books)
                {
                    Console.WriteLine($"{b.Id},{b.Title},{b.Price},{b.AuthorName}");
                }
            }
        }

        private static bool IsOK(string s)
        {
            return s.Contains("张");
        }*/
    }
}