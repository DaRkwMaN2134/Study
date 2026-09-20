using ConfigurationLibrary;
using DTOLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static ConfigurationLibrary.Interfaces;

namespace BotStart
{
    public partial class Bot
    {
        async Task ShowMainMenu(long chatId)
        {
            await _botClient.SendMessage(chatId, "Выберите действие", replyMarkup: BuildMainMenuKeyboard());
        }



        async Task ShowBalanceAsync(long chatId, IBotWrite botWrite)
        {
            var user = await botWrite.GetUserAsync(chatId);
            await _botClient.SendMessage(chatId, $"Ваш баланс равен: {user.Points}");
        }

        async Task ShowMyLinkAsync(long chatId)
        {
            await _botClient.SendMessage(chatId, $"Ваша личная ссылка: t.me/{_botUsername}?start=ref_{chatId}");
        }

        async Task ShowServicesAsync(long chatId, int messageId)
        {
            await _botClient.EditMessageText(chatId, messageId, "{переход на новое меню кнопок}", replyMarkup: BuildServicesKeyboard());
        }

        async Task ShowHelpAsync(long chatId, int messageId)
        {
            await _botClient.EditMessageText(chatId, messageId, "{переход на новое меню кнопок}", replyMarkup: BuildHelpKeyboard());
        }

        async Task ShowSettingsAsync(long chatId, int messageId)
        {
            await _botClient.EditMessageText(chatId, messageId, "{переход на новое меню кнопок}", replyMarkup: BuildSettingsKeyboard());
        }

        async Task ShowMyReferralsAsync(long chatId, IBotWrite botWrite)
        {
            var refsList = await botWrite.GetReferralsAsync(chatId);
            string answer = string.Empty;
            if (refsList.Any() == false)
            {
                await _botClient.SendMessage(chatId, "Список пуст");
                return;
            }

            foreach (var refs in refsList)
            {
                answer += $"{refs.InvitedUser.Username ?? refs.InvitedUser.FirstName ?? "Без имени"} {refs.InvitedUser.RegisteredAt.ToString("dd.MM.yyyy")}\n";
            }
            await _botClient.SendMessage(chatId, $"Ваши рефералы\n" + answer);
        }
    }
}