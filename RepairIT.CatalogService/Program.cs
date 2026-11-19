using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.Data;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));

// EF Core + SQLite (base commune, voir RepairIT.Data)
builder.Services.AddDbContext<RepairItDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RepairItDb")));

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
