using CountryDictionaryApiApp.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(); // новая строка

var app = builder.Build();

app.MapGet("/", () => "Server is Run!");

app.MapGet("/ping", () => "pong!");

app.MapGet("/country", async (ApplicationDbContext db) => await db.Countries.ToListAsync());

app.MapPost("/country", async (Country country, ApplicationDbContext db) =>
{
    await db.Countries.AddAsync(country);
    await db.SaveChangesAsync();
    return country;
});

app.MapGet("/api/country/{code:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.Countries.FirstOrDefaultAsync(c => c.Id == id);
});

//id
app.MapGet("/api/country/{id:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.Countries.FirstOrDefaultAsync(c => c.Id == id);
});

//code
app.MapGet("/api/country/code/{code:alpha}", async (string code, ApplicationDbContext db) =>
{
    Country country = await db.Countries.FirstOrDefaultAsync(d => d.ISO31661Alpha2Code == code || d.ISO31661Alpha3Code == code || d.ISO31661NumericCode == code);
    return country;
});


app.Run();
