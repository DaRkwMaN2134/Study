using DataLibrary;
using FileIOLibrary;
using ParserLibrary;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Bot
{
    private static readonly string BotToken = "";
    private static CancellationTokenSource? _scheduleCts = null;
    private static bool _isParsing = false;
    private static bool _isScheduleEnabled = false;
    private static DateTime _lastRunTime;
    private static int _lastRunCount;
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

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        User me = await botClient.GetMe();
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
            await botClient.SendMessage(chatId, "Доступные команды: /start, /help, /run_parser, /schedule_on, /schedule_off, /status", cancellationToken: cancellationToken);
        }

        else if (messageText.StartsWith("/run_parser"))
        {
            if (_isParsing)
            {
                await botClient.SendMessage(chatId, "Парсинг уже выполняется, подождите.");
                return;
            }
            await botClient.SendMessage(chatId, "Начался парсинг карточек", cancellationToken: cancellationToken);
            _ = Task.Run(() => ParserCommandAsync(botClient, update, cancellationToken, chatId));
            await botClient.SendMessage(chatId, "Парсинг запущен в фоне...");

        }

        else if (messageText.StartsWith("/schedule_on"))
        {
            if (_isScheduleEnabled)
            {
                await botClient.SendMessage(chatId, "⚠️ Расписание уже включено.");
                return;
            }
            _isScheduleEnabled = true;
            _scheduleCts = new CancellationTokenSource();
            await botClient.SendMessage(chatId, "✅ Расписание включено. Парсинг будет запускаться каждую 1 минуту.");

            _ = Task.Run(async () =>
            {
                using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
                while (_isScheduleEnabled)
                {
                    try
                    {
                        await timer.WaitForNextTickAsync(_scheduleCts.Token);
                        await ParserCommandAsync(botClient, null!, _scheduleCts.Token, chatId);
                        _lastRunTime = DateTime.Now;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка в расписании: {ex.Message}");
                    }
                }
                _isScheduleEnabled = false;
            });
        }

        else if (messageText.StartsWith("/schedule_off"))
        {
            if (!_isScheduleEnabled)
            {
                await botClient.SendMessage(chatId, "⚠️ Расписание уже выключено.");
                return;
            }

            _isScheduleEnabled = false;
            _scheduleCts?.Cancel();
            await botClient.SendMessage(chatId, "⏹ Расписание отключается...");
        }

        else if (messageText.StartsWith("/status"))
        {
            await botClient.SendMessage(chatId, $"Статус расписания:{_isScheduleEnabled}");
            await botClient.SendMessage(chatId, $"Последнее количество товаров:{_lastRunCount}");
            await botClient.SendMessage(chatId, $"🕒Последний запуск::{_lastRunTime}");
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
        int totalProcessed = 0;
        int notifyStep = 100;
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
        try
        {

            _isParsing = true;
            foreach (var categoryUrl in categories)
            {
                string url = categoryUrl;
                while (!string.IsNullOrEmpty(url))
                {
                    var html = await client.HttpRequestAsync(url);
                    var cards = await parser.ParseCategoryAsync(html, categoryUrl);
                    allCards.AddRange(cards);
                    totalProcessed += cards.Count;
                    Console.Write($"Обработано карточек - {allCards.Count}\n");
                    if (totalProcessed % notifyStep < cards.Count)
                    {
                        await botClient.SendMessage(chatId, $"⏳ Обработано {totalProcessed} товаров...");
                    }
                    url = parser.ParseUrl(html, url);
                }
            }
        }
        finally
        {
            _isParsing = false;
        }
        Console.Write("Карточки спарсены");
        await botClient.SendMessage(chatId, $"Всего спарсено карточек {allCards.Count}", cancellationToken: cancellationToken);
        await excel.ExcelOutput(allCards);
        _lastRunCount = allCards.Count;
        _lastRunTime = DateTime.Now;

        await using var stream = File.OpenRead("Card.xlsx");
        await botClient.SendDocument(chatId, stream);
    }
}