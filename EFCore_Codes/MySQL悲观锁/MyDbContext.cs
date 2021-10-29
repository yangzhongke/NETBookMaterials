using Microsoft.EntityFrameworkCore;
using System;

namespace MySQL悲观锁
{
    class MyDbContext:DbContext
    {
        public DbSet<House> Houses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var connectionString = "server=localhost;user=root;password=root;database=ef";
            var serverVersion = new MySqlServerVersion(new Version(5, 6));
            optionsBuilder.UseMySql(connectionString,serverVersion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
