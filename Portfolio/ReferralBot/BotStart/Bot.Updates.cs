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
                await HandleCallbackQueryAsync(callbackQuery, cancellationToken);
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
                    long refererlId = 0;
                    var botWrite = scope.ServiceProvider.GetRequiredService<IBotWrite>();
                    if (messageText.Contains("ref_"))
                    {
                        string[]? parse = messageText.Split('_');
                        if (long.TryParse(parse[1], out long result))
                        {
                            refererlId = result;
                        }

                        await botWrite.CheckOrCreateUserAsync(registration);

                        if (refererlId == chatId)
                        {
                            await _botClient.SendMessage(chatId, "Нельзя пригласить себя");
                            return;
                        }
                        else
                        {
                            await botWrite.GetReferrerAsync(chatId, refererlId);
                            await botWrite.SetUserPointsAsync(refererlId);
                        }
                    }
                    await botWrite.CheckOrCreateUserAsync(registration);
                }
                await ShowMainMenu(chatId);

            }
        }
    }
}
