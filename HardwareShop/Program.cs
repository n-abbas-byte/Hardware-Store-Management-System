using HardwareShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");


void ConfigureDb(DbContextOptionsBuilder options)
{
    options.UseSqlite(connectionString);
}


builder.Services.AddDbContext<AppDbContext>(ConfigureDb);
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/products", GetAllProducts);
app.MapPost("/api/products", AddProduct);
app.Run();

IResult GetAllProducts(AppDbContext db)
{
    return Results.Ok(db.Products.ToList());
}

IResult AddProduct([FromBody] Product product, AppDbContext db)
{
    db.Products.Add(product);
    db.SaveChanges();
    return Results.Created($"/api/products/{product.Id}", product);
}