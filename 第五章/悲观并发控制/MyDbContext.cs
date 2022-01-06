using Microsoft.EntityFrameworkCore;
class MyDbContext : DbContext
{
    public DbSet<House> Houses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=adfa3_ioz09_08nljo;database=ef",
            new MySqlServerVersion(new Version(8, 6, 20)));
        //optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}