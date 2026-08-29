using DataLibrary;
using HtmlAgilityPack;
using ParserLibrary;
using FileIOLibrary;

class Program
{
    static Http_Client client = new Http_Client();
    static Html_Parser parser = new Html_Parser();
    static Excel_Output excel = new Excel_Output();
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
                allCards.AddRange(cards);
                Console.Write($"Обработано карточек - {allCards.Count}\n");
                url = parser.ParseUrl(html, url);
            }
        }
        await excel.ExcelOutput(allCards);
    }
}
