using DataBaseLibrary;
using DataLibrary;
using FileIOLibrary;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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

        ExcelPackage.License.SetNonCommercialPersonal("Learning");
        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Цитаты");
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Цитата";
        worksheet.Cells[1, 3].Value = "Автор";
        worksheet.Cells[1, 4].Value = "Теги";

        worksheet.View.FreezePanes(2, 1);


        quoteList = await clientRequest.ParseQuotesWithScrollAsync();

        db.Database.EnsureCreated();
        db.quotes.AddRange(quoteList); // чтобы записать в базу данных, нужно выполнить миграцию dotnet ef migrations add InitialCreate и dotnet ef database update
        await db.SaveChangesAsync();

        await jsonInput.JsonWriteAsync(file3, quoteList);

        var quotes = db.quotes;
        int row = 2;
        foreach (var quote in quotes)
        {
            worksheet.Cells[row, 1].Value = quote.id;
            worksheet.Cells[row, 2].Value = quote.text;
            worksheet.Cells[row, 3].Value = quote.author;
            worksheet.Cells[row, 4].Value = string.Join(", ", quote.tags);
            row++;
        }
        string[] col = ["ID", "Цитата", "Автор", "Теги"];
        for (int i = 0; i < col.Length; i++)
        {

            worksheet.Cells[1, 1, 1, i+1].Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, i+1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[1, 1, 1, i+1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            worksheet.Cells[1, 1, row-1, i+1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, row-1, i+1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, row-1, i+1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, row-1, i+1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        }
        worksheet.Cells[1, 1, row-1, 4].AutoFitColumns();
        worksheet.Cells[1, 1, row-1, 4].AutoFilter = true;
        worksheet.Cells[1, 1, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Cells[2, 1, row - 1, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        await package.SaveAsAsync(new FileInfo("quotes.xlsx"));
    }

    static public async Task Main()
    {
        Program start = new Program();
        await start.Parser();

        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}