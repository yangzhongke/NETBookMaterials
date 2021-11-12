using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
	public DbSet<Book> Books { get; set; }
	public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
	{
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
	}
}