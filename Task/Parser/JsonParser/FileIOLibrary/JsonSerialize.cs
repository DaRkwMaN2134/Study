using System.Text.Json;
using DataLibrary;

namespace FileIOLibrary
{
    public class JsonSerialize
    {
        public async Task<Post> JsonWriteAsync(string filepath, string jsonString)
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
    }
}
