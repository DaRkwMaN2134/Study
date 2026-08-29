using System.Net;

namespace ParserLibrary
{
    public class Http_Client
    {
        public async Task<string> HttpRequestAsync(string url)
        {
            int maxRetries = 4;
            int attempt = 0;
            while (attempt < maxRetries)
            {
                try
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
                    if (responce.IsSuccessStatusCode)
                    {
                        return await responce.Content.ReadAsStringAsync();
                    }

                    if (responce.StatusCode >= HttpStatusCode.InternalServerError || responce.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        attempt++;
                        if (attempt >= maxRetries)
                        {
                            throw new HttpRequestException($"Не удалось получить ответ после {maxRetries} попыток");
                        }
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt))); // 2, 4, 8 секунд
                        continue;
                    }
                    else
                    {
                        throw new HttpRequestException($"Ошибка запроса: {responce.StatusCode}");
                    }
                }
                catch (Exception ex) when (attempt < maxRetries - 1)
                {
                    attempt++;
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                }
            }
            throw new Exception("Превышено число попыток");
        }
    }
}