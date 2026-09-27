
using ParserLibrary;
class Program()
{
    HTTP_Client clientRequest = new HTTP_Client();
    async Task Parser()
    {
        await clientRequest.HTTPLoginRequestAsync("https://the-internet.herokuapp.com");
    }

    static public async Task Main()
    {
        Program start = new Program();
        await start.Parser();

        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}