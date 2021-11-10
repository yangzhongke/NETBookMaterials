using TestDbContext ctx = new TestDbContext();
var books = ctx.Books.Where(b => b.PubTime.Year > 2010).Take(3);
foreach (var b in books)
{
    Console.WriteLine(b.Title);
}