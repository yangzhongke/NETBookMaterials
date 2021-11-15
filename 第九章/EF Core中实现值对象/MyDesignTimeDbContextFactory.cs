using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
{
    public TestDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<TestDbContext> builder = new();
        string connStr = "Data Source=.;Initial Catalog=valueobj1;Integrated Security=true";
        builder.UseSqlServer(connStr);
        return new TestDbContext(builder.Options);
    }
}