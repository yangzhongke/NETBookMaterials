using Microsoft.EntityFrameworkCore;

namespace BgService1
{
    public class TestDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options)
           : base(options)
        {

        }
    }
}
