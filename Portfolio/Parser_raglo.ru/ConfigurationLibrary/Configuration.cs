using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ConfigurationLibrary
{
    public class Configuration
    {
        public string TokenLoadConfiguration()
        {
            string jsonText = File.ReadAllText("appsettings.json");

            using JsonDocument doc = JsonDocument.Parse(jsonText);

            string token = doc.RootElement
                .GetProperty("Bot_Token")
                .GetProperty("Token")
                .GetString();
            //string token = configuration.GetConnectionString("Token");
            return token;
        }

        public int IntervalLoadConfiguration()
        {
            string jsonText = File.ReadAllText("appsettings.json");

            using JsonDocument doc = JsonDocument.Parse(jsonText);

            int interval = doc.RootElement
                .GetProperty("ScheduleIntervalMinutes")
                .GetInt32();
            return interval;
        }

        public void editIntervalLoadConfiguration(int newInterval)
        {
            string jsonText = File.ReadAllText("appsettings.json");
            JsonNode? _root = JsonNode.Parse(jsonText);

            if (_root != null)
            {
                _root["ScheduleIntervalMinutes"] = newInterval;
            }
            string updatedJson = _root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("appsettings.json", updatedJson);
        }
    }
}
