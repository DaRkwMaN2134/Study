using ConfigurationLibrary;
using DTOLibrary;
using DataLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static ConfigurationLibrary.Interfaces;

namespace BotStart
{
    public partial class Bot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<Bot> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CancellationTokenSource _cts = new();
        private string? _botUsername;

        public Bot(ITelegramBotClient botClient, ILogger<Bot> logger, IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            _botClient = botClient;
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task runBotAsync()
        {
            
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            await _botClient.DropPendingUpdates();

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: _cts.Token
                );

            var me = await _botClient.GetMe();
            _botUsername = me.Username;
            Console.WriteLine(_botUsername);

        }

        async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            //await _logger.LogErrorAsync("Произошла ошибка", exception);
            await Task.CompletedTask;
        }

        private InlineKeyboardMarkup BuildMainMenuKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("Баланс", "balance"), InlineKeyboardButton.WithCallbackData("Моя ссылка", "my_link"), InlineKeyboardButton.WithCallbackData("Услуги", "services") },
                new[] { InlineKeyboardButton.WithCallbackData("Помощь", "help"), InlineKeyboardButton.WithCallbackData("Настройки", "settings"), InlineKeyboardButton.WithCallbackData("Мои рефералы", "my_refs")}
            });
        }

        private InlineKeyboardMarkup BuildServicesKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("❌ Главное меню ", "menu_back") }
            });
        }

        private InlineKeyboardMarkup BuildHelpKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("❌ Главное меню ", "menu_back") }
            });
        }

        private InlineKeyboardMarkup BuildSettingsKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("❌ Главное меню ", "menu_back") }
            });
        }
    }
}
