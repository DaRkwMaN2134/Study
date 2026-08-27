using DataLibrary;
using HtmlAgilityPack;
using ParserLibrary;

class Program
{
    static Http_Client client = new Http_Client();
    static Html_Parser parser = new Html_Parser();
    static async Task Main()
    {
        string url = "https://raglo.ru/catalog/dushevye-trapy/";
        var allCards = new List<Card>();
        while (!string.IsNullOrEmpty(url))
        {
            var html = await client.HttpRequestAsync(url);
            var cards = await parser.ParseCategoryAsync(html);
            allCards.AddRange(cards);
            Console.WriteLine(url);
            url = parser.ParseUrl(html, url);
           
        }
        foreach (var card in allCards)
        {
            Console.WriteLine($"Категория: {card.categoryname}\nАртикль: {card.article}\nСсылка: {card.pictureurl}\nЦена: {card.price}\nОписание: {card.description}\n");
        }
    }
}
