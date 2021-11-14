using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
{
	public TestDbContext CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<TestDbContext> builder = new();
		string connStr = Environment.GetEnvironmentVariable("ConnectionStrings:Default");
		builder.UseSqlServer(connStr);
		return new TestDbContext(builder.Options);
	}
}