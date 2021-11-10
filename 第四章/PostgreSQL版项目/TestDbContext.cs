using Microsoft.EntityFrameworkCore;
class TestDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=ef;Username=postgres;Password=123456");
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}