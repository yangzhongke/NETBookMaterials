using Microsoft.EntityFrameworkCore.Design;

namespace 值对象在EFCore中的实现1
{
    internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<TestDbContext> builder = new ();
            string connStr = "Data Source=.;Initial Catalog=valueobj1;Integrated Security=true";
            builder.UseSqlServer(connStr);
            return new TestDbContext(builder.Options);
        }
    }
}
