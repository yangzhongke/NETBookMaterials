using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BooksEFCore
{
    internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {

        public MyDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MyDbContext> builder = new ();
            //builder.UseSqlServer("Server=.;Database=demo8;Trusted_Connection=True;");
            string connStr = Environment.GetEnvironmentVariable("ConnectionStrings:BooksEFCore");
            builder.UseSqlServer(connStr);
            return new MyDbContext(builder.Options);
        }
    }
}
