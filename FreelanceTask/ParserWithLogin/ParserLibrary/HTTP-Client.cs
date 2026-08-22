using System.Net;

namespace ParserLibrary
{
    public class HTTP_Client
    {
        static HttpClient client = new HttpClient();
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
            var response = await client.PostAsync("https://the-internet.herokuapp.com/authenticate", content); // обязательна ссылка, которая ведет на проверку пароля(проверяется через devtool(f12) в браузере)

            string html = await client.GetStringAsync("https://the-internet.herokuapp.com/secure");



            Console.WriteLine($"Status \n {response.StatusCode} \n");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"Html код после входа: \n {html}");
            }
        }
    }
}
