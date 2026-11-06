using HardwareShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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
app.MapPost("/api/auth/login", Login);
app.MapGet("/api/products/{id}", GetProductsByID);
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

IResult Login([FromBody] LoginRequest Request, IConfiguration config)
{
    if (Request.Username == "admin" && Request.Password == "1234")
    {
        var key = config["JWT:Key"];
        var keyBytes = Encoding.UTF32.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, Request.Username));
        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        var token = new JwtSecurityToken(issuer: config["JWT:Key"], audience: config["JWT:Audience"], claims: claims, expires: DateTime.Now.AddHours(8));
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = tokenString, role = "admin" });
    }
    else
        return Results.Unauthorized();
}

IResult GetProductsByID(int id, AppDbContext db)
{
    var products = db.Products.FirstOrDefault(p => p.Id == id);
    if (products != null)
        return Results.Ok(products);
    else
        return Results.NotFound();
}