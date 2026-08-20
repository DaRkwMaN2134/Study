using DataModel;
using HtmlAgilityPack;
using Microsoft.Playwright;
using System.Net;
using System.Text.Json;
namespace Parser
{
    public class HTTP_Client
    {
        static HttpClient client = new HttpClient();

        public async Task<string> HTTPJsonplaceholderRequestAsync()
        {
            string html = "";
            string url = "https://jsonplaceholder.typicode.com/todos/1";
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                html = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Успешно! Длина полученного HTML-кода: " + html.Length);
                Console.WriteLine(string.Join(",", html));
                return html;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }

        public async Task<string> HTTPBookstoscrapeRequestAsync(string url)
        {
            //string url = "https://books.toscrape.com/catalogue/page-1.html";
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var html = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("Успешно! Длина полученного HTML-кода: " + html.Length);
                //Console.WriteLine(string.Join(",", html));
                return html;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }

        public async Task HTTPLoginRequestAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var handler = new HttpClientHandler();

            handler.CookieContainer = new CookieContainer();

            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("Referer", "https://the-internet.herokuapp.com/login");


            var loginData = new Dictionary<string, string>
            {
                { "username", "tomsmith" },
                { "password", "SuperSecretPassword!" }
            };

            var content = new FormUrlEncodedContent(loginData);
            var response = await client.PostAsync("https://the-internet.herokuapp.com/authenticate", content);

            string html = await client.GetStringAsync("https://the-internet.herokuapp.com/secure");



            Console.WriteLine($"Вывод для responce: \n {response} \n");

            Console.WriteLine($"Вывод для html: \n {html}");
        }

        public async Task HTTPCSRFRequestAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("Referer", "http://quotes.toscrape.com/login");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var loginHtml = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(loginHtml);
            var CSRFNode = doc.DocumentNode.SelectSingleNode("//input[@name='csrf_token']");
            string currentCSRF = CSRFNode.GetAttributeValue("value", "");
            string CSRF = "";
            if (!string.IsNullOrEmpty(currentCSRF))
            {
                CSRF = currentCSRF?.Trim() ?? "CSRF отсутствует";
            }



            var loginData = new Dictionary<string, string>
            {
                { "username", "admin" },
                { "password", "password!" },
                { "csrf_token", CSRF}
            };
            var enterContent = new FormUrlEncodedContent(loginData);
            var enterResponse = await client.PostAsync("http://quotes.toscrape.com/login", enterContent);
            string enterHtml = await client.GetStringAsync("http://quotes.toscrape.com/");

            Console.WriteLine($"Вывод для responce: \n {enterResponse} \n");
            Console.WriteLine(enterHtml);
            var cookies = handler.CookieContainer.GetCookies(new Uri("http://quotes.toscrape.com"));
            foreach (System.Net.Cookie cookie in cookies)
            {
                Console.WriteLine($"{cookie.Name} = {cookie.Value}");
            }
        }
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
                    
                    if(textNode != null)
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
                    Console.WriteLine($"Автор: {quote.Author}\n Теги: {string.Join(",", quote.Tags)}\n Текст: {quote.Text} \n"); 
                }
            }
            finally
            {
                if(browser != null)
                {
                    await browser.CloseAsync();
                    await browser.DisposeAsync();
                }
                if(playwright != null)
                {
                    playwright.Dispose();
                    await Task.Delay(500);
                }
            }
            return quoteList;
        }

    }
}
