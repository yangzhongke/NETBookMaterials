using Microsoft.EntityFrameworkCore;

class TestDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.;Database=demo5;Trusted_Connection=True";
        optionsBuilder.UseSqlServer(connStr);
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}