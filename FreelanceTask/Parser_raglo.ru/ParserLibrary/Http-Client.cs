using System.Net;

namespace ParserLibrary
{
    public class Http_Client
    {
        public async Task<string> HttpRequestAsync(string url)
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0...");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml");
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; YandexBot/3.0; +http://yandex.com)");
            //client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");


            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var responce = await client.SendAsync(request);
            /*Console.WriteLine($"Первый ответ от сервера: \n{responce}");

            if (responce.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            {
                foreach (var cookie in cookieHeaders)
                {
                    Console.WriteLine($"Вывод куки: {cookie}");
                }
            }
            Console.WriteLine($"Вывод html: {html}");*/
            var html = await responce.Content.ReadAsStringAsync();
            
            return html;
        }
            
    }
}
