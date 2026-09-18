using ConfigurationLibrary;
using DTOLibrary;
using Npgsql.Replication.PgOutput.Messages;
using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly bool isBlocked;

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.MyChatMember is { } mychatmember)
            {
                return;
            }
            if (update.CallbackQuery is { } callbackQuery)
            {
                return;
            }
            if (update.Message is not { } message || message.Text is not { } messageText)
            {
                return;
            }

            long chatId = message.Chat.Id;


            if (isBlocked)
            {
                await botClient.SendMessage(chatId, "Вы заблокированы");
                return;
            }

            if (messageText.StartsWith("/start"))
            {

                var registration = new UserRegistrationDto
                {
                    ChatId = message.Chat.Id,
                    Username = message.Chat.Username,
                    FirstName = message.Chat.FirstName
                };
                using (var scope = _serviceScopeFactory.CreateAsyncScope())
                {
                    var botWrite = scope.ServiceProvider.GetRequiredService<IBotWrite>();
                    await botWrite.GetOrCreateUserAsync(registration);
                }
                await ShowMainMenu(chatId);

            }
        }
    }
}
