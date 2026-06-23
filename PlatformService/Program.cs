using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Mappers;
using PlatformService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext using InMemory database
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));

// Add repository to the container
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Add Mapperly to the container
builder.Services.AddSingleton<PlatformMappers>();

// Add validation to the container
builder.Services.AddValidation();

var app = builder.Build();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/platforms",  (IPlatformRepo repo, PlatformMappers mapper) =>
{
   IEnumerable<Platform> platforms = repo.GetAllPlatforms();
   return Results.Ok(platforms.Select(mapper.MapToReadDto));
})
.WithName("GetPlatforms");

app.MapGet("/platforms/{id}",  (IPlatformRepo repo, PlatformMappers mapper, int id) =>
{
    Platform? platform = repo.GetPlatformById(id);
    return platform != null ? Results.Ok(mapper.MapToReadDto(platform)) : Results.NotFound();
})
.WithName("GetPlatformById");

app.MapPost("/platforms", (IPlatformRepo repo, PlatformMappers mapper, PlatformCreateDto platformCreateDto) => 
{
    Platform platform = mapper.MapToModel(platformCreateDto);
    repo.CreatePlatform(platform);
    repo.SaveChanges();
    return Results.Created($"/platforms/{platform.Id}", mapper.MapToReadDto(platform));
}).WithName("CreatePlatform");

app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (var url in app.Urls)
    {
        Console.WriteLine($"Listening on {url}");
    }
});

app.Run();
