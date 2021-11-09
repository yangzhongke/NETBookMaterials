using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 充血模型在EFCore中的实现1
{
    internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<TestDbContext> builder = new ();
            string connStr = "Data Source=.;Initial Catalog=chongxue;Integrated Security=true";
            builder.UseSqlServer(connStr);
            return new TestDbContext(builder.Options);
        }
    }
}
