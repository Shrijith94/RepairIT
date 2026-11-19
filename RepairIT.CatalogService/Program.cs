using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));

// EF Core + SQLite (une base par microservice)
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatalogDb")));

// Injection des dépendances
builder.Services.AddScoped<CatalogDbInitializer>();
builder.Services.AddScoped<IOrdinateurService, OrdinateurService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialisation de la base de données
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<CatalogDbInitializer>().InitialiserAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
