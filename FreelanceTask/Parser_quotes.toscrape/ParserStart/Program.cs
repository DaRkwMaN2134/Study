using DataBaseLibrary;
using DataLibrary;
using FileIOLibrary;
using ParserLibrary;



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
        db.quotes.AddRange(quoteList); // чтобы записать в базу данных, нужно выполнить миграцию dotnet ef migrations add InitialCreate и dotnet ef database update
        await db.SaveChangesAsync();
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