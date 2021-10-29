using Microsoft.EntityFrameworkCore;

namespace EFcoreOneToOneBug
{
    class MyDbContext:DbContext
    {
        public DbSet<A> A { get; set; }
        public DbSet<B> B { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Database=testbug;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<A>().HasOne(x => x.B).WithOne(x=>x.A);
        }
    }
}
