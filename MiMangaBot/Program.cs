// JaveragesLibrary/Program.cs

using Microsoft.EntityFrameworkCore;
using MiMangaBot.Data;
using MiMangaBot.Services;
using MiMangaBot.Domain.Repositories;
using MiMangaBot.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System.Text.Json;

System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registrar servicios
builder.Services.AddScoped<IMangaRepository, SupabaseMangaRepository>();
builder.Services.AddScoped<MangaGeneratorService>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Manga Generator API", 
        Version = "v1",
        Description = "API para generar datos de mangas únicos y almacenarlos en Firebase"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();