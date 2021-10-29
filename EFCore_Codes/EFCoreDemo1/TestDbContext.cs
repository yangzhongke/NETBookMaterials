using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace EFCoreDemo1
{
    class TestDbContext:DbContext
    {
        /*
        public static readonly ILoggerFactory MyLoggerFactory
                = LoggerFactory.Create(builder => { builder.AddConsole(); });*/

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //;MultipleActiveResultSets=true
            string connStr = "Server=.;Database=demo1;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connStr);
            // optionsBuilder.UseLoggerFactory(MyLoggerFactory);

            /*
            optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=ef",
                new MySqlServerVersion(new Version(5, 6, 0)));
            */
            /*
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=ef;Username=postgres;Password=123456");*/
            optionsBuilder.LogTo(msg=> {
                if (!msg.Contains("RelationalEventId.CommandExecuted")) return;
                Console.WriteLine(msg);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}