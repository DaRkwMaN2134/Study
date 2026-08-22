using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace DataBaseLibrary
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=quotes.db");
        }
    }
}
