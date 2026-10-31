using HardwareShop;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
void ConfigureDb(DbContextOptionsBuilder options)
{
    options.UseSqlite(connectionString);
}
builder.Services.AddDbContext<AppDbContext>(ConfigureDb);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

