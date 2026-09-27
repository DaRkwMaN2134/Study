using System.Text;
using System.Text.Json;
using DataLibrary;

namespace ParserLibrary
{
    public class HTTP_Client
    {
        static HttpClient client = new HttpClient();
        public async Task<string> HTTPRequestAsync()
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
        public async Task<PostRequest> HttpPostAsync(PostRequest postToSend)
        {
            string jsonBody = JsonSerializer.Serialize(postToSend);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://jsonplaceholder.typicode.com/posts", content);
            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var createdUser = JsonSerializer.Deserialize<PostRequest>(responseJson);
                Console.WriteLine($"Ответ сервера: {responseJson}");
                return createdUser;
            }
            else
            {
                Console.WriteLine($"Вход не удался, ошибка: {response.StatusCode}");
                return null;
            }
        }

        public async Task<PostRequest> PostRequestAsync(string url, PostRequest postToSend)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            var response = await client.SendAsync(request);
            string jsonBody = JsonSerializer.Serialize(postToSend);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            string responseJson = await response.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<PostRequest>(responseJson);
            Console.WriteLine(responseJson);
            return createdUser;
        }
    }
}
