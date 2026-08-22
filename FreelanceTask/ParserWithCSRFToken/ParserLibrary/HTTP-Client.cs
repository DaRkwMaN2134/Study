using System.Net;
using HtmlAgilityPack;

namespace ParserLibrary
{
    public class HTTP_Client
    {
        static HttpClient client = new HttpClient();
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


            Console.WriteLine($"Status:\n{enterResponse.StatusCode}\n");
            Console.WriteLine($"CSRF-токен: \n{CSRF}\n");
            var cookies = handler.CookieContainer.GetCookies(new Uri("http://quotes.toscrape.com"));
            foreach (System.Net.Cookie cookie in cookies)
            {
                Console.WriteLine($"Cookie: {cookie.Name} = {cookie.Value}");
            }

            Console.WriteLine($"\nHTML после входа: \n{enterHtml}");
        }
    }
}
