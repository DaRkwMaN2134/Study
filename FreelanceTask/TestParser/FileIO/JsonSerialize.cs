using DataModel;
using System.Text.Json;


namespace FileIO
{
    public class JsonSerialize
    {
        public async Task<Post> JsonPostWriteAsync(string filepath, string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Post posts = JsonSerializer.Deserialize<Post>(jsonString, options);
            string json = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filepath, json);
            return posts;
        }

        public async Task JsonBookWriteAsync(string filepath, List<Book> books)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(books, options);
            await File.WriteAllTextAsync(filepath, json);
        }

        public async Task JsonQuoteWriteAsync(string filepath, List<Quote> quotes)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(quotes, options);
            await File.WriteAllTextAsync(filepath, json);
        }
    }
}