using ParserLibrary;
using FileIOLibrary;

class Program()
{
    HTTP_Client clientRequest = new HTTP_Client();
    JsonSerialize jsonInput = new JsonSerialize();
    string file1 = "Post.json";
    async Task Parser()
    {
        var jsonString1 = await clientRequest.HTTPRequestAsync();
        var post1 = await jsonInput.JsonWriteAsync(file1, jsonString1);
        Console.WriteLine($"Title: {post1.Title}");
        Console.WriteLine($"Completed: {post1.Completed}\n\n");
    }

    static public async Task Main()
    {
        Program start = new Program();
        await start.Parser();

        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}