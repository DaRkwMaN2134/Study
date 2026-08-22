using System.Text.Json;
using DataLibrary;

namespace FileIOLibrary
{
    public class JsonSerialize
    {
        public async Task JsonWriteAsync(string filepath, List<Quote> quotes)
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
