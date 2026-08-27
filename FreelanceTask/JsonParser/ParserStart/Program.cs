using DataLibrary;
using FileIOLibrary;
using ParserLibrary;

class Program()
{
    HTTP_Client clientRequest = new HTTP_Client();
    JsonSerialize jsonInput = new JsonSerialize();
    string file1 = "Post.json";
    async Task Parser()
    {
        var jsonString1 = await clientRequest.HTTPRequestAsync();
        var post1 = await jsonInput.JsonWriteAsync(file1, jsonString1);



        var newPost = new PostRequest
        {
            UserId = 1,
            Title = "My Title",
            Body = "My Body"
        };
        var createdPost = await clientRequest.PostRequestAsync("https://jsonplaceholder.typicode.com/posts", newPost);
        Console.WriteLine($"Создан пост с Id: {createdPost.id}");
    }


    static public async Task Main()
    {
        Program start = new Program();
        await start.Parser();

        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}