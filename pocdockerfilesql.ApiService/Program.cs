using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();


// Add DbContext service
builder.AddSqlServerDbContext<Context>("sqldb");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (Context ctx) =>
{
    return ctx.Forecasts.ToList();
});

app.MapDefaultEndpoints();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var forecasts = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ));

    context.Forecasts.AddRange(forecasts);
    context.SaveChanges();
}

app.Run();

public class WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class Context(DbContextOptions options) : DbContext(options)
{
    public DbSet<WeatherForecast> Forecasts => Set<WeatherForecast>();
}