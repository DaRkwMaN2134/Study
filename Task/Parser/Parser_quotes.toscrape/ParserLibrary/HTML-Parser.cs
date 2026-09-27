using Microsoft.Playwright;
using DataLibrary;

namespace ParserLibrary
{
    public class HTML_Parser
    {
        public async Task<List<Quote>> ParseQuotesWithScrollAsync()
        {
            IPlaywright playwright = null;
            IBrowser browser = null;

            string text = "";
            string author = "";

            List<Quote> quoteList = new List<Quote>();

            try
            {
                playwright = await Playwright.CreateAsync();
                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false
                });
                var page = await browser.NewPageAsync();
                page.SetDefaultTimeout(60000);
                var headers = new Dictionary<string, string>
                {
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36" },
                };
                await page.SetExtraHTTPHeadersAsync(headers);
                await page.GotoAsync("http://quotes.toscrape.com/scroll");
                await page.WaitForSelectorAsync(".quote");

                int scrollCount = 0;
                int previousCount = 0;
                int sameCountAttempts = 0;

                while (scrollCount < 30)
                {
                    await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
                    await Task.Delay(500);
                    scrollCount++;

                    var currentQuotes = await page.QuerySelectorAllAsync(".quote");
                    int currentCount = currentQuotes.Count;

                    if (currentCount == previousCount)
                    {
                        sameCountAttempts++;
                        if (sameCountAttempts >= 2 && scrollCount > 2)
                            break;
                    }
                    else
                    {
                        sameCountAttempts = 0;
                    }

                    previousCount = currentCount;
                }
                var elements = await page.QuerySelectorAllAsync(".quote");
                foreach (var element in elements)
                {
                    Quote quote = new Quote();
                    var textNode = await element.QuerySelectorAsync(".text");
                    var authorNode = await element.QuerySelectorAsync(".author");
                    var tagNode = await element.QuerySelectorAllAsync(".tag");

                    if (textNode != null)
                    {
                        text = await textNode.TextContentAsync();
                    }

                    if (authorNode != null)
                    {
                        author = await authorNode.TextContentAsync();
                    }

                    var tags = new List<string>();
                    if (tagNode != null)
                    {

                        foreach (var tag in tagNode)
                        {
                            tags.Add(await tag.TextContentAsync());
                        }

                    }
                    quote = new Quote(text, author, tags);
                    quoteList.Add(quote);

                }
                Console.WriteLine($"Общее кол-во цитат: {elements.Count}");
                foreach (Quote quote in quoteList)
                {
                    Console.WriteLine($"Автор: {quote.author}\n Теги: {string.Join(",", quote.tags)}\n Текст: {quote.text} \n");
                }
            }
            finally
            {
                if (browser != null)
                {
                    await browser.CloseAsync();
                    await browser.DisposeAsync();
                }
                if (playwright != null)
                {
                    playwright.Dispose();
                    await Task.Delay(500);
                }
            }
            return quoteList;
        }
    }
}
