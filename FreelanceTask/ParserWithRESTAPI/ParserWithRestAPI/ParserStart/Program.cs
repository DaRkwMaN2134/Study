using DataBaseLibrary;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

class Program
{
    static AppDbContext db = new AppDbContext();

    static async Task Main()
    {
        var start = new Program();

        decimal? cachedPrice = await start.GetCachedPriceAsync("bitcoin_price", 5);
        if (cachedPrice.HasValue)
        {
            Console.WriteLine($"Цена из кэша: {cachedPrice.Value} USD");
            return;
        }

        decimal apiPrice = await start.GetBitcoinPriceAsync();
        if (apiPrice > 0)
        {
            await start.SavePriceAsync("bitcoin_price", apiPrice);
            Console.WriteLine($"Цена из API: {apiPrice} USD");
        }
        else
        {
            Console.WriteLine("Не удалось получить цену.");
        }
    }

    async Task<decimal> GetBitcoinPriceAsync()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            string json = await client.GetStringAsync("https://api.coingecko.com/api/v3/coins/bitcoin");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<BitcoinPrice>(json, options);

            if (data?.market_data?.current_price?.TryGetValue("usd", out decimal price) == true)
                return price;
            else
                return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка запроса к API: {ex.Message}");
            return 0;
        }
    }

    async Task<decimal?> GetCachedPriceAsync(string key, int maxAgeMinutes)
    {
        try
        {
            var allEntries = db.cache.ToList();
            var entry = allEntries.FirstOrDefault(c => c.key == key);
            if (entry != null)
            {
                if (entry.updated_at > DateTime.UtcNow.AddMinutes(-maxAgeMinutes))
                {
                    if (decimal.TryParse(entry.value, out var price))
                        return price;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения кэша: {ex.Message}");
            return null;
        }
    }
    async Task SavePriceAsync(string key, decimal price)
    {
        try
        {
            var entry = db.cache.Find(key);
            if (entry != null)
            {
                entry.value = price.ToString();
                entry.updated_at = DateTime.UtcNow;
            }
            else
            {
                db.cache.Add(new cache(key, price.ToString(), DateTime.UtcNow));
            }
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения в кэш: {ex.Message}");
        }
    }
    public class BitcoinPrice
    {
        public MarketData market_data { get; set; }
    }

    public class MarketData
    {
        public Dictionary<string, decimal> current_price { get; set; }
    }
}