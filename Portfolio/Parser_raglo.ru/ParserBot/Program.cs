using DataLibrary;
using FileIOLibrary;
using ParserLibrary;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Bot
{
    private static readonly string BotToken = "8144039959:AAHku7u_maIMq83blIDXP6QIsWZCplga240";
    private static bool _isParsing = false;
    static Http_Client client = new Http_Client();
    static Html_Parser parser = new Html_Parser();
    static Excel_Output excel = new Excel_Output();

    static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClient(BotToken);
        using var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        // Исправлено: errorHandler вместо pollingErrorHandler
        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandlePollingErrorAsync,   // <-- исправлено
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        // Исправлено: GetMe() вместо GetMeAsync()
        User me = await botClient.GetMe();           // <-- исправлено
        Console.WriteLine($"Бот {me.FirstName} запущен и слушает сообщения...");

        Console.ReadLine();
        cts.Cancel();
    }

    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        Console.WriteLine($"Получено сообщение: '{messageText}' от пользователя {chatId}");

        if (messageText.StartsWith("/start"))
        {
            await botClient.SendMessage(chatId, "Привет! Я бот, который умеет парсить сайты. Напиши /help для списка команд.", cancellationToken: cancellationToken);
        }
        else if (messageText.StartsWith("/help"))
        {
            await botClient.SendMessage(chatId, "Доступные команды: /start, /help, /run_parser", cancellationToken: cancellationToken);
        }
        else if (messageText.StartsWith("/run_parser"))
        {
            if (_isParsing == true)
            {
                await botClient.SendMessage(chatId, "Парсинг уже запущен в фоне, дождитесь завершения");
                return;
            }

            _isParsing = true;
            await botClient.SendMessage(chatId, "Начался парсинг карточек", cancellationToken: cancellationToken);
            _ = Task.Run(() => ParserCommandAsync(botClient, update, cancellationToken, chatId));
            await botClient.SendMessage(chatId, "Парсинг запущен в фоне...");
        }

        else
        {
            await botClient.SendMessage(chatId, $"Я не знаю команду '{messageText}'", cancellationToken: cancellationToken);
        }
    }

    static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Произошла ошибка: {exception.Message}");
        return Task.CompletedTask;
    }

    static async Task ParserCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, long chatId)
    {
        var allCards = new List<Card>();
        var categories = new List<string>
            {
            "https://raglo.ru/catalog/dushevye-trapy/",
            "https://raglo.ru/catalog/kukhnya/dozatory-/",
            "https://raglo.ru/catalog/po-seriyam/",
            "https://raglo.ru/catalog/polotentsesushiteli/",
            "https://raglo.ru/catalog/splenka/zapchasti-s/",
            "https://raglo.ru/catalog/aksessuary-dlya-smesiteley/",
            "https://raglo.ru/catalog/kukhonnye-moyki/",
            "https://raglo.ru/catalog/aksessuary-dlya-vannoy-komnaty/"
            };

        foreach (var categoryUrl in categories)
        {
            string url = categoryUrl;
            while (!string.IsNullOrEmpty(url))
            {
                var html = await client.HttpRequestAsync(url);
                var cards = await parser.ParseCategoryAsync(html, categoryUrl);
                allCards.AddRange(cards);
                Console.Write($"Обработано карточек - {allCards.Count}\n");
                url = parser.ParseUrl(html, url);
            }
        }
        Console.Write("Карточки спарсены");
        await botClient.SendMessage(chatId, $"Всего спарсено карточек {allCards.Count}", cancellationToken: cancellationToken);
        await excel.ExcelOutput(allCards);
        await using var stream = File.OpenRead("Card.xlsx");
        await botClient.SendDocument(chatId, stream);
    }
}