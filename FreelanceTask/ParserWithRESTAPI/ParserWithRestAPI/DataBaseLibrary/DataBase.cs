using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Npgsql;
using DataLibrary;
namespace DataBaseLibrary
{
    public class AppDbContext : DbContext
    {
        public DbSet<cache> cache { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            string connectionToDataBase = config.GetConnectionString("Postgres");
            optionsBuilder.UseNpgsql(connectionToDataBase);
        }
    }
}