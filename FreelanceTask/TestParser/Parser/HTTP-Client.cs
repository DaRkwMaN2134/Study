using System.Net;
using System.Net.Http;
using System.Text.Json;
using DataModel;
namespace Parser
{
    public class HTTP_Client
    {
        HttpClient client = new HttpClient();
        public string html = "";
        string url = "https://jsonplaceholder.typicode.com/todos/1";
        public async Task<string> HTTPRequestAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                var response = await client.SendAsync(request);
                html = await client.GetStringAsync(url);
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
    }
}
