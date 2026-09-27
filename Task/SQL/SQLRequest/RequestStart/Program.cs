using Microsoft.Extensions.Configuration;
using Npgsql;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = config.GetConnectionString("Postgres");

        await SelectQuotesAsync(connectionString);

    }

    static async Task SelectQuotesAsync(string connectionString)
    {
        var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
       
        var request = 
        "SELECT author, COUNT(*) " +
        "FROM quotes " +
        "GROUP BY author " +
        "ORDER BY COUNT(*) DESC";

        var cmd = new NpgsqlCommand(request, conn);
        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            string author = reader.GetString(0);
            int quotes = reader.GetInt32(1);

            Console.WriteLine($"Автор: {author} — {quotes} цитаты");
            Console.WriteLine(new string('-', 40));
        }
    }
}