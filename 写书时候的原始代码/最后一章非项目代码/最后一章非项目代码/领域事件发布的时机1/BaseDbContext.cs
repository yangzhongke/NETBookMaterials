using MediatR;
using Microsoft.EntityFrameworkCore;

namespace 领域事件发布的时机1
{
    public abstract class BaseDbContext : DbContext
    {
        private IMediator mediator;

        public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            this.mediator = mediator;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("Don not call SaveChanges, please call SaveChangesAsync instead.");
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var domainEntities = this.ChangeTracker.Entries<IDomainEvents>()
                            .Where(x => x.Entity.GetDomainEvents().Any());
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents()).ToList();
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
