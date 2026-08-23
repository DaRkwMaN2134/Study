using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataBaseLibrary
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quote> quotes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=****;Database=QuotesDB");
        }
    }
}
