using DataLibrary;
using HtmlAgilityPack;
using ParserLibrary;

class Program
{
    static Http_Client client = new Http_Client();
    static Html_Parser parser = new Html_Parser();
    static async Task Main()
    {
        var allCards = new List<Card>();
        var categories = new List<string>
        {
        "https://raglo.ru/catalog/dushevye-trapy/",
        "https://raglo.ru/catalog/kukhnya/dozatory-/",
        "https://raglo.ru/catalog/po-seriyam/",
        "https://raglo.ru/catalog/polotentsesushiteli/",
        "https://raglo.ru/catalog/splenka/zapchasti-s/",
        "https://raglo.ru/catalog/aksessuary-dlya-smesiteley/",
        "https://raglo.ru/catalog/kukhonnye-moyki/",
        "https://raglo.ru/catalog/aksessuary-dlya-vannoy-komnaty/"
        };

        foreach (var categoryUrl in categories)
        {
            string url = categoryUrl;
            while (!string.IsNullOrEmpty(url))
            {
                var html = await client.HttpRequestAsync(url);
                var cards = await parser.ParseCategoryAsync(html, categoryUrl);
                Console.WriteLine(url);
                allCards.AddRange(cards);

                url = parser.ParseUrl(html, url);
            }
        }
        foreach (var card in allCards)
        {
            Console.WriteLine($"Категория: {card.categoryname}\nАртикль: {card.article}\nСсылка: {card.pictureurl}\nЦена: {card.price}\nОписание: \n{card.description}\n");
        }
    }
}
