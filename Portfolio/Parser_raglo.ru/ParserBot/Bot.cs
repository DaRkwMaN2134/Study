using ConfigurationLibrary;
using DataLibrary;
using FileIOLibrary;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using ParserLibrary;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Bot
{

    private readonly ILogger _logger;
    private readonly IHttpClient _httpClient;
    private readonly IHtmlParser _htmlParser;
    private readonly IExcelOutput _excelOutput;
    private readonly string _botToken;
    private readonly Configuration _config;
    public Bot(ILogger logger, IHttpClient httpClient, IHtmlParser htmlParser, IExcelOutput excelOutput, Configuration config)
    {
        _logger = logger;
        _httpClient = httpClient;
        _htmlParser = htmlParser;
        _excelOutput = excelOutput;
        _botToken = config.TokenLoadConfiguration();
        _config = config;
    }

    private static CancellationTokenSource? _scheduleCts = null;
    private static CancellationTokenSource? _parserCts = null;
    private static bool _isParsing = false;
    private static bool isWaiting = false;
    private static bool _isScheduleEnabled = false;
    private static DateTime _lastRunTime;
    private static int _lastRunCount;
    private static readonly ConcurrentDictionary<long, string> _userState = new ConcurrentDictionary<long, string>();


    static public async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IHttpClient, Http_Client>();
        services.AddSingleton<IHtmlParser, Html_Parser>();
        services.AddSingleton<IExcelOutput, Excel_Output>();
        services.AddSingleton<ILogger, FileLogger>();
        services.AddSingleton<Configuration>();
        services.AddSingleton<Bot>();


        var serviceProvider = services.BuildServiceProvider();

        var bot = serviceProvider.GetRequiredService<Bot>();
        await bot.runBotAsync();
    }
    async Task runBotAsync()
    {
        var botClient = new TelegramBotClient(_botToken);
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
        try
        {
            User me = await botClient.GetMe();
            await _logger.LogAsync($"Бот {me.FirstName} запущен");
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync($"Бот", ex);
        }

        Console.ReadLine();
        cts.Cancel();
        await _logger.LogAsync($"Бот выключен");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message || message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        if (_userState.TryGetValue(chatId, out var state))
        {
            if (state == "awaiting_interval")
            {
                await schedule_edit_CommandAsync(botClient, chatId, messageText);
                return;
            }
        }


        await _logger.LogAsync($"Получено сообщение: '{messageText}' от пользователя {chatId}");

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
                await _logger.LogAsync($"Парсинг уже выполняется, подождите");
                return;
            }
            await botClient.SendMessage(chatId, "Начался парсинг карточек", cancellationToken: cancellationToken);
            await _logger.LogAsync($"Начался парсинг карточек");
            _ = Task.Run(() => ParserCommandAsync(botClient, cancellationToken, chatId));
            await botClient.SendMessage(chatId, "Парсинг запущен в фоне...");
            await _logger.LogAsync($"Парсинг запущен в фоне...");

        }


        else if (messageText.StartsWith("/schedule_on"))
        {
            await schedule_on_CommandAsync(botClient, chatId);
        }

        else if (messageText.StartsWith("/schedule_off"))
        {
            await schedule_off_CommandAsync(botClient, chatId);
        }

        else if (messageText.StartsWith("/schedule_edit"))
        {
            await botClient.SendMessage(chatId, $"Введите желаемый интервал обновления");
            _userState[chatId] = "awaiting_interval";
        }

        else if (messageText.StartsWith("/status"))
        {
            await status_CommandAsync(botClient, chatId);
        }

        else if (messageText.StartsWith("/stop_parser"))
        {
            if (_isParsing == true)
            {
                try
                {
                    _parserCts?.Cancel();                }
                catch (Exception ex)
                {
                    await _logger.LogErrorAsync("Ошибка при остановке парсинга", ex);
                }
                finally
                {
                    _isParsing = false;
                }

            }

        }

        else
        {
            await botClient.SendMessage(chatId, $"Я не знаю команду '{messageText}'", cancellationToken: cancellationToken);
        }
    }

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogErrorAsync("Произошла ошибка", exception);
        return Task.CompletedTask;
    }


    async Task schedule_edit_CommandAsync(ITelegramBotClient botClient, long chatId, string messageText)
    {
        if (int.TryParse(messageText, out int interval) == false)
        {
            await botClient.SendMessage(chatId, $"Ошибка. Введите число");
            return;
        }
        else
        {
            _config.editIntervalLoadConfiguration(interval);
            await botClient.SendMessage(chatId, $"✅ Интервал обновлён: {interval} минут.");
        }
        _userState.TryRemove(chatId, out var removedState);

    }



    async Task schedule_on_CommandAsync(ITelegramBotClient botClient, long chatId)
    {
        var interval = TimeSpan.FromMinutes(_config.IntervalLoadConfiguration());

        if (_isScheduleEnabled)
        {
            await botClient.SendMessage(chatId, "⚠️ Расписание уже включено.");
            return;
        }
        _isScheduleEnabled = true;
        _scheduleCts = new CancellationTokenSource();
        await botClient.SendMessage(chatId, $"✅ Расписание включено. Парсинг будет запускаться с промежутком {interval}");

        _ = Task.Run(async () =>
        {
            using var timer = new PeriodicTimer(interval);
            while (_isScheduleEnabled)
            {
                try
                {
                    await timer.WaitForNextTickAsync(_scheduleCts.Token);
                    await ParserCommandAsync(botClient, _scheduleCts.Token, chatId);
                    _lastRunTime = DateTime.Now;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    await _logger.LogErrorAsync("Произошла ошибка", ex);
                }
            }
            _isScheduleEnabled = false;
        });
    }

    async Task schedule_off_CommandAsync(ITelegramBotClient botClient, long chatId)
    {
        if (!_isScheduleEnabled)
        {
            await botClient.SendMessage(chatId, "⚠️ Расписание уже выключено.");

            return;
        }

        _isScheduleEnabled = false;
        _scheduleCts?.Cancel();
        _scheduleCts?.Dispose();
        _scheduleCts = null;
        await botClient.SendMessage(chatId, "⏹ Расписание отключается...");
    }



    async Task status_CommandAsync(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendMessage(chatId, $"Статус расписания:{_isScheduleEnabled}");
        await botClient.SendMessage(chatId, $"Статус расписания:{_config.IntervalLoadConfiguration()}");
        await botClient.SendMessage(chatId, $"Последнее количество товаров:{_lastRunCount}");
        await botClient.SendMessage(chatId, $"🕒Последний запуск::{_lastRunTime.ToString("HH:mm:ss dd.MM.yyyy")}");
    }


    async Task ParserCommandAsync(ITelegramBotClient botClient, CancellationToken cancellationToken, long chatId)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Learning");

        List<Card> batch = new List<Card>();
        int batchSize = 50;
        int currentRow = 2;
        int totalProcessed = 0;
        int notifyStep = 100;

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

        _parserCts = new CancellationTokenSource();

        try
        {
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Карточки");
            List<string> headeades = new List<string>{
                "Имя категории",
                "Артикль",
                "Url-картинки",
                "Цена",
                "Описание"};

            for (int i = 0; i < headeades.Count; i++)
            {
                sheet.Cells[1, i + 1].Value = headeades[i];
            }
            sheet.View.FreezePanes(2, 1);

            if (_isParsing)
            {
                return;
            }

            try
            {

                _isParsing = true;
                foreach (var categoryUrl in categories)
                {
                    string url = categoryUrl;
                    while (!string.IsNullOrEmpty(url))
                    {
                        var html = await _httpClient.HttpRequestAsync(url, _parserCts);
                        var cards = await _htmlParser.ParseCategoryAsync(html, categoryUrl, _parserCts);
                        batch.AddRange(cards);
                        totalProcessed += cards.Count;

                        if (batch.Count >= batchSize)
                        {
                            await _excelOutput.AppendCardsAsync(sheet, batch, currentRow);
                            currentRow += batch.Count;
                            batch.Clear();
                        }
                        if (totalProcessed % notifyStep < cards.Count)
                        {
                            await _logger.LogAsync($"Обработано карточек - {totalProcessed}");
                            await botClient.SendMessage(chatId, $"⏳ Обработано {totalProcessed} товаров...");
                        }
                        url = _htmlParser.ParseUrl(html, url);
                    }
                }
                if (batch.Count > 0)
                {
                    await _excelOutput.AppendCardsAsync(sheet, batch, currentRow);
                }

                await package.SaveAsAsync(new FileInfo("Card.xlsx"));
            }
            catch(Exception ex)
            {
                await _logger.LogErrorAsync(ex.Message);
            }
            await _logger.LogAsync($"Карточки спарсены");
            await botClient.SendMessage(chatId, $"Всего спарсено карточек {totalProcessed}", cancellationToken: cancellationToken);
            _lastRunCount = totalProcessed;
            _lastRunTime = DateTime.Now;
            await SendFileAsync(botClient, chatId);
        }
        catch (OperationCanceledException)
        {
            await _logger.LogAsync("Парсинг был остановлен пользователем.");
            await botClient.SendMessage(chatId, "⏹ Парсинг остановлен.");
            return;
        }
        finally
        {
            _parserCts?.Cancel();
            _isParsing = false;
        }
    }

    async Task SendFileAsync(ITelegramBotClient botClient, long chatId)
    {
        if (!File.Exists("Card.xlsx"))
        {
            await botClient.SendMessage(chatId, "Файл не создан, проверьте парсинг.");
            return;
        }
        try
        {
            await using var stream = File.OpenRead("Card.xlsx");
            await botClient.SendDocument(chatId, stream);
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Произошла ошибка", ex);
        }
    }
}