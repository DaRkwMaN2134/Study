using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;

namespace ConfigurationLibrary
{
    public class Configuration
    {
        public string LoadConfiguration()
        {
            string jsonText = File.ReadAllText("appsettings.json");

            using JsonDocument doc = JsonDocument.Parse(jsonText);

            string token = doc.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("Token")
                .GetString();
            //string token = configuration.GetConnectionString("Token");
            return token;
        }
    }
}
