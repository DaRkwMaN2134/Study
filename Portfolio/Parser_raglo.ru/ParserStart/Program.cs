using ConfigurationLibrary;
using DataLibrary;
using FileIOLibrary;
using HtmlAgilityPack;
using ParserLibrary;

class Program
{
    private readonly ILogger _logger;
    private readonly IHttpClient _httpClient;
    private readonly IHtmlParser _htmlParser;
    private readonly IExcelOutput _excelOutput;
    public Program(ILogger logger, IHttpClient httpClient, IHtmlParser htmlParser, IExcelOutput excelOutput)
    {
        _logger = logger;
        _httpClient = httpClient;
        _htmlParser = htmlParser;
        _excelOutput = excelOutput;
    }
    static CancellationToken token = new CancellationToken();
    async Task Main()
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
                var html = await _httpClient.HttpRequestAsync(url, token);
                var cards = await _htmlParser.ParseCategoryAsync(html, categoryUrl);
                allCards.AddRange(cards);
                Console.Write($"Обработано карточек - {allCards.Count}\n");
                url = _htmlParser.ParseUrl(html, url);
            }
        }
        await _excelOutput.ExcelOutput(allCards);
    }
}
