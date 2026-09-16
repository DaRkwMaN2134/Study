using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connString));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
