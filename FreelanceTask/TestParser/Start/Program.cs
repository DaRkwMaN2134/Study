using DataModel;
using FileIO;
using Parser;

HTTP_Client clientRequest = new HTTP_Client();
JsonSerialize jsonInput = new JsonSerialize();
HTML_Parser htmlParser = new HTML_Parser();
string file1 = "post1.json";
string file2 = "post2.json";

var jsonString1 = await clientRequest.HTTPJsonplaceholderRequestAsync();
var post1 = await jsonInput.JsonPostWriteAsync(file1, jsonString1);
Console.WriteLine($"Title: {post1.Title}");
Console.WriteLine($"Completed: {post1.Completed}\n\n");



var html2 = await clientRequest.HTTPBookstoscrapeRequestAsync();
var list1 = htmlParser.BookParse(html2);
if (list1 != null)
{
    await jsonInput.JsonBookWriteAsync(file2, list1);
    foreach (var book in list1)
    {
        Console.WriteLine($"{book.Title} — {book.Price}");
    }
}
else
{
    Console.WriteLine($"Парсинг пустой");
}