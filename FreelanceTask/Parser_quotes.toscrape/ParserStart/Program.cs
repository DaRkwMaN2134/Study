using DataBaseLibrary;
using DataLibrary;
using FileIOLibrary;
using Microsoft.EntityFrameworkCore;
using ParserLibrary;
using System.Text.Json;



class Program()
{
    JsonSerialize jsonInput = new JsonSerialize();
    HTML_Parser clientRequest = new HTML_Parser();
    AppDbContext db = new AppDbContext();
    string file3 = "Quote.json";

    async Task Parser()
    {
        List<Quote> quoteList = new List<Quote>();
        quoteList = await clientRequest.ParseQuotesWithScrollAsync();
        db.Database.EnsureCreated();
        db.Quotes.AddRange(quoteList); // чтобы записать в базу данных, нужно выполнить миграцию dotnet ef migrations add InitialCreate и dotnet ef database update
        await db.SaveChangesAsync();

        var allQuotes = db.Quotes.ToList();

        var einsteinQuotes = db.Quotes.Where(q => q.Author == "Albert Einstein").ToList();

        var grouped = db.Quotes.GroupBy(q => q.Author)
                               .Select(g => new { Author = g.Key, Count = g.Count() })
                               .ToList();
        await jsonInput.JsonWriteAsync(file3, quoteList);
    }

    static public async Task Main()
    {
        Program start = new Program();
        await start.Parser();

        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}