using Microsoft.EntityFrameworkCore;
using Users.Domain;

namespace Users.Infrastructure
{
    public class UserDbContext:DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<UserLoginHistory> LoginHistories { get; private set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
