using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Telegram.Bot.Types;

namespace ParserBot
{
    public class StateStorage
    {
        private bool _isAuthorized = false;
        private long _authorizedChatId = 0;
        private int _activeMenuMessageId = 0;
        private readonly string path = "state.json";
        private readonly string sample = "{\r\n  \"ChatId\": 1833123665,\r\n  \"IsAuthorized\": true,\r\n  \"ActiveMenuMessageId\": 0 \r\n}";
        public StateStorage()
        {
            LoadState();
        }
        public void LoadState()
        {
            if (File.Exists(path) == true)
            {
                try
                {
                    string jsonString = File.ReadAllText(path);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    BotState json = JsonSerializer.Deserialize<BotState>(jsonString, options);
                    _authorizedChatId = json.ChatId;
                    _isAuthorized = json.IsAuthorized;
                    _activeMenuMessageId = json.ActiveMenuMessageId;
                    Console.WriteLine($"{json.ChatId}, {json.IsAuthorized}, {json.ActiveMenuMessageId} ");
                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                File.Create(path).Close();
                File.WriteAllText(path, sample);
            }
        }

        private async Task SaveStateAsync()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            BotState state = new BotState
            {
                ChatId = _authorizedChatId,
                IsAuthorized = _isAuthorized,
                ActiveMenuMessageId = _activeMenuMessageId
            };

            string json = JsonSerializer.Serialize(state, options);
            await File.WriteAllTextAsync(path, json);
        }

        public bool IsAuthorized()
        {
            return _isAuthorized;
        }

        public async Task Authorize(long chatId)
        {
            _authorizedChatId = chatId;
            _isAuthorized = true;
            await SaveStateAsync();
        }

        public int GetActiveMenuId()
        {
            return _activeMenuMessageId;
        }

        public async Task SetActiveMenu(int activeMenuMessageId)
        {
            _activeMenuMessageId = activeMenuMessageId;
            await SaveStateAsync();
        }

        public async Task ClearActiveMenu()
        {
            _activeMenuMessageId = 0;
            await SaveStateAsync();
        }
    }
}
