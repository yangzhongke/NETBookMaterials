/*
using TestDbContext ctx = new TestDbContext();
Book b = new Book { AuthorName = "Bill Gates", Title = "Zack, Cool guy!", 
    Price = 9.9, PubTime = new DateTime(2020, 12, 30) };
ctx.Books.Add(b);
Console.WriteLine($"保存前，Id={b.Id}");
await ctx.SaveChangesAsync();
Console.WriteLine($"保存后，Id={b.Id}");*/
/*
using TestDbContext ctx = new TestDbContext() ;
Console.WriteLine("****1*****");
Author a1 = new Author { Name = "杨中科" };
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
Console.WriteLine($"保存前，Id={a2.Id}");*/
using TestDbContext ctx = new TestDbContext();
var books = ctx.Books.Where(b => b.PubTime.Year > 2010).Take(3);
foreach (var b in books)
{
 Console.WriteLine(b.Title);
}
