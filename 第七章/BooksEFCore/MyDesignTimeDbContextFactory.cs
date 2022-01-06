using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

class MyDesignTimeDbContextFactory:IDesignTimeDbContextFactory<MyDbContext>
{
	public MyDbContext CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<MyDbContext> builder = new();
		string connStr = Environment.GetEnvironmentVariable("ConnectionStrings:BooksEFCore");
		builder.UseSqlServer(connStr);
		return new MyDbContext(builder.Options);
	}
}