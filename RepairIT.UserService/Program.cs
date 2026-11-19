using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Configuration;
using RepairIT.Data;
using RepairIT.UserService.Data;
using RepairIT.UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));

// EF Core + SQLite (base commune, voir RepairIT.Data)
builder.Services.AddDbContext<RepairItDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RepairItDb")));

// Injection des dépendances
builder.Services.AddScoped<UserDbInitializer>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialisation de la base de données
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<UserDbInitializer>().InitialiserAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
