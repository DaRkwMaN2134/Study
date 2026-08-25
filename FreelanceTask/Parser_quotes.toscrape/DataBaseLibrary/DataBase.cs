using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DataBaseLibrary
{
    public class AppDbContext : DbContext
    {

        public DbSet<Quote> quotes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            string connectionString = config.GetConnectionString("Postgres");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
