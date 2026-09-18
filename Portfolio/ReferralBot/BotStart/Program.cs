using BotStart;
using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using static ConfigurationLibrary.Interfaces;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres"),
        b => b.MigrationsAssembly("ConfigurationLibrary")
    ));
string token = builder.Configuration["Bot_Token:Token"];
builder.Services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(token));
builder.Services.AddScoped<IBotWrite, BotDataOutput>();
builder.Services.AddSingleton<Bot>();

var app = builder.Build();

var bot = app.Services.GetRequiredService<Bot>();

_ = bot.runBotAsync();

app.Run();
