using DataModel;
using System.Text.Json;


namespace FileIO
{
    public class JsonSerialize
    {
        public async Task<Post> JsonWriteAsync(string filepath, string html)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Post posts = JsonSerializer.Deserialize<Post>(html, options);
            string json = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filepath, json);
            return posts;
        }
    }
}

