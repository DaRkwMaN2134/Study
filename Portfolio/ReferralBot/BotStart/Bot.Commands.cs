using ConfigurationLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace BotStart
{
    public partial class Bot
    {
        async Task ShowMainMenu(long chatId)
        {
            await _botClient.SendMessage(chatId, "Выберите действие", replyMarkup: BuildMainMenuKeyboard());
        }
    }
}
