using MediatR;
using Microsoft.EntityFrameworkCore;

namespace 领域事件发布的时机1
{
    public class UserDbContext: BaseDbContext
    {
        public DbSet<User> Users { get; private set; }

        public UserDbContext(DbContextOptions<UserDbContext> options, 
            IMediator mediator): base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
