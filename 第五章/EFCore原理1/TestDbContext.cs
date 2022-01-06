using Microsoft.EntityFrameworkCore;
class TestDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Server=.;Database=demo1;Trusted_Connection=True";
        optionsBuilder.UseSqlServer(connStr);
        optionsBuilder.LogTo(msg => {
            if(msg.Contains("CommandExecuted"))
                Console.WriteLine(msg);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}