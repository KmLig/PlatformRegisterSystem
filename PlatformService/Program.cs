using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Mappers;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add DbContext using InMemory database
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));

// Add repository to the container
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Add Mapperly to the container
builder.Services.AddSingleton<PlatformMappers>();

// Add HttpClient to the container
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (var url in app.Urls)
    {
        Console.WriteLine($"Listening on {url}");
    }
});

app.Run();
